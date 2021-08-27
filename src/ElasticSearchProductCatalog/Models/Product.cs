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

        public List<ProductProperty> Properties { get; set; } = new();

        public Dictionary<string, string> PropertyLookup { get; set; } = new();
    }
}