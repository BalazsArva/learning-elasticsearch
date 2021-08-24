using System.Collections.Generic;

namespace ElasticSearchProductCatalog.Models
{
    public class Product
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public bool IsActive { get; set; }

        public decimal Price { get; set; }

        public Dictionary<string, string> Properties { get; set; } = new();
    }

    public class ProductSearchModel
    {
        public SearchParameter<bool> IsActive { get; set; }

        public TextSearchParameter<string> Title { get; set; }

        public TextSearchParameter<string> Category { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }

    public class SearchParameter<TValue>
    {
        public TValue Value { get; set; }
    }

    public class TextSearchParameter<TValue>
    {
        public TValue Value { get; set; }

        public bool Exact { get; set; }
    }
}