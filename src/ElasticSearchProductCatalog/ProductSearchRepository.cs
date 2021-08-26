using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ElasticSearchProductCatalog.Models;
using Nest;

namespace ElasticSearchProductCatalog
{
    public class ProductSearchRepository
    {
        private const string KeywordFieldSuffix = "keyword";

        private readonly ElasticClient elasticClient;

        public ProductSearchRepository()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200", UriKind.Absolute))
                .DefaultIndex("products")
                .DefaultMappingFor<Product>(m =>
                {
                    return m
                        .IndexName("products")
                        .IdProperty(p => p.Id);
                });

            // Only needed to be able to check the outgoing request in the DebugInformation property of the search response. Can leave out when not needed.
            settings = settings.DisableDirectStreaming();

            elasticClient = new ElasticClient(settings);
        }

        public async Task InsertAsync(Product product)
        {
            await elasticClient.IndexDocumentAsync(product);
        }

        public async Task<IEnumerable<Product>> SearchAsync(ProductSearchModel searchModel)
        {
            var results = await elasticClient
                .SearchAsync<Product>(search => search
                    .Query(query =>
                    {
                        var subqueries = new List<QueryContainer>();

                        if (searchModel.Title is not null && !string.IsNullOrWhiteSpace(searchModel.Title.Value))
                        {
                            subqueries.Add(
                                GetTextQuery(query, p => p.Title, searchModel.Title));
                        }

                        if (searchModel.Category is not null && !string.IsNullOrWhiteSpace(searchModel.Category.Value))
                        {
                            subqueries.Add(
                                GetTextQuery(query, p => p.Category, searchModel.Category));
                        }

                        if (searchModel.IsActive is not null)
                        {
                            subqueries.Add(
                                query.Term(p => p.IsActive, searchModel.IsActive.Value));
                        }

                        return subqueries
                            .DefaultIfEmpty(query.MatchAll())
                            .Aggregate((partialResult, current) => partialResult & current);
                    })
                    .Skip((searchModel.Page - 1) * searchModel.PageSize)
                    .Take(searchModel.PageSize));

            return results.Documents;
        }

        public async Task<IEnumerable<string>> GetPropertiesByCategoryAsync(string category)
        {
            const string aggregationName = "PropertyKeys";

            var categorySearchParam = new TextSearchParameter { SearchMethod = TextSearchMethod.Equals, Value = category };

            var results = await elasticClient
                .SearchAsync<Product>(search => search
                    .Query(query => GetTextQuery(query, p => p.Category, categorySearchParam))
                    .Aggregations(aggs => aggs.Terms(aggregationName, a => a.Field("properties.key.keyword").Size(250)))
                    .Size(0));

            if (results.Aggregations.TryGetValue(aggregationName, out var aggr) && aggr is BucketAggregate bucketAggregate)
            {
                return bucketAggregate.Items.Cast<KeyedBucket<object>>().Select(b => (string)b.Key).ToList();
            }

            return Enumerable.Empty<string>();
        }

        private QueryContainer GetTextQuery(QueryContainerDescriptor<Product> query, Expression<Func<Product, string>> propertySelector, TextSearchParameter parameter)
        {
            if (parameter.SearchMethod == TextSearchMethod.Equals)
            {
                // The '.Suffix(KeywordFieldSuffix)' enables precise equality filtering. The "property.keyword" is a non-analyzed (~not tokenized/altered)
                // value, which is the original, raw value that the field originally contained prior to any processing. We can use this field to check for
                // precise equality when using term queries.
                var suffixMethodCallExpression = Expression.Call(typeof(SuffixExtensions), nameof(SuffixExtensions.Suffix), Array.Empty<Type>(), propertySelector.Body, Expression.Constant(KeywordFieldSuffix));
                var propertyKeywordSelectorExpression = Expression.Lambda<Func<Product, object>>(suffixMethodCallExpression, propertySelector.Parameters);

                return query.Term(propertyKeywordSelectorExpression, parameter.Value);
            }

            if (parameter.SearchMethod == TextSearchMethod.ContainsEntirePhrase)
            {
                return query.MatchPhrase(mp => mp.Field(propertySelector).Query(parameter.Value));
            }

            if (parameter.SearchMethod == TextSearchMethod.ContainsAllTokens)
            {
                return query.Match(m => m.Field(propertySelector).Operator(Operator.And).Query(parameter.Value));
            }

            if (parameter.SearchMethod == TextSearchMethod.ContainsAnyToken)
            {
                return query.Match(m => m.Field(propertySelector).Operator(Operator.Or).Query(parameter.Value));
            }

            //
            //if (parameter.SearchMethod == TextSearchMethod.ContainsTokenPrefix)
            //{
            //    return query.Prefix(pfx => pfx.Field(propertySelector).Value(parameter.Value))
            //}

            throw new ArgumentOutOfRangeException(nameof(TextSearchParameter.SearchMethod));
        }
    }
}