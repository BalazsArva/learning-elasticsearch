namespace ElasticSearchProductCatalog.Models
{
    public enum TextSearchMethod
    {
        Equals,
        ContainsAllTokens,
        ContainsAnyTokens,
        ContainsEntirePhrase,
        ContainsTokenPrefix,
    }
}