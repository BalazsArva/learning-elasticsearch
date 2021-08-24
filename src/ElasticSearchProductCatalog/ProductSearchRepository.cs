using System;
using System.Collections.Generic;
using System.Linq;
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

                        if (searchModel.Title is not null)
                        {
                            // The '.Suffix(KeywordFieldSuffix)' enables precise equality filtering. The "property.keyword" is a non-analyzed (~not tokenized/altered)
                            // value, which is the original, raw value that the field originally contained prior to any processing. We can use this field to check for
                            // precise equality when using term queries.
                            var titleQuery = searchModel.Title.Exact
                                ? query.Term(t => t.Title.Suffix(KeywordFieldSuffix), searchModel.Title.Value)
                                : query.Match(m => m.Field(p => p.Title).Operator(Operator.And).Query(searchModel.Title.Value));

                            subqueries.Add(titleQuery);
                        }

                        if (searchModel.Category is not null)
                        {
                            var categoryQuery = searchModel.Category.Exact
                                ? query.Term(t => t.Category.Suffix(KeywordFieldSuffix), searchModel.Category.Value)
                                : query.Match(m => m.Field(p => p.Category).Operator(Operator.And).Query(searchModel.Category.Value));

                            subqueries.Add(categoryQuery);
                        }

                        if (searchModel.IsActive is not null)
                        {
                            var isActiveFilter = query.Term(p => p.IsActive, searchModel.IsActive.Value);

                            subqueries.Add(isActiveFilter);
                        }

                        return subqueries
                            .DefaultIfEmpty(query.MatchAll())
                            .Aggregate((partialResult, current) => partialResult & current);
                    })
                    .Skip((searchModel.Page - 1) * searchModel.PageSize)
                    .Take(searchModel.PageSize));

            return results.Documents;
        }
    }
}