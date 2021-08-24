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
}