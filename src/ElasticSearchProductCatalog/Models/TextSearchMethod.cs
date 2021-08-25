namespace ElasticSearchProductCatalog.Models
{
    public enum TextSearchMethod
    {
        Equals,
        ContainsAllTokens,
        ContainsAnyToken,
        ContainsEntirePhrase,
        ContainsTokenPrefix,
    }
}