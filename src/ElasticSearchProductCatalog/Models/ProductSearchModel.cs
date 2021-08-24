namespace ElasticSearchProductCatalog.Models
{
    public class ProductSearchModel
    {
        public SearchParameter<bool> IsActive { get; set; }

        public TextSearchParameter Title { get; set; }

        public TextSearchParameter Category { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}