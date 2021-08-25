using System;
using System.Threading.Tasks;
using ElasticSearchProductCatalog.Models;
using Nest;
using Newtonsoft.Json;

namespace ElasticSearchProductCatalog
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var repo = new ProductSearchRepository();

            foreach (var product in SampleProducts.GetProducts())
            {
                await repo.InsertAsync(product);
            }

            while (true)
            {
                Console.Clear();

                await SearchAsync(repo);

                Console.WriteLine();
                Console.WriteLine("Done. Press any key to search again or 'q' to quit.");
                Console.WriteLine();

                if (Console.ReadKey().Key == ConsoleKey.Q)
                {
                    break;
                }
            }
        }

        private static TextSearchParameter GetSearchParameterFromConsole(string parameterName)
        {
            TextSearchMethod searchMethod;
            var searchValue = string.Empty;

            while (true)
            {
                Console.WriteLine($"Search mode for '{parameterName}':");
                Console.WriteLine($"0 - Don't search for {parameterName}");
                Console.WriteLine($"1 - Equals");
                Console.WriteLine($"2 - Contains entire phrase");
                Console.WriteLine($"3 - Contains all tokens");
                Console.WriteLine($"4 - Contains any token");

                var mode = Console.ReadLine().Trim();
                if (mode == "0")
                {
                    return null;
                }

                if (mode == "1")
                {
                    searchMethod = TextSearchMethod.Equals;
                    break;
                }
                else if (mode == "2")
                {
                    searchMethod = TextSearchMethod.ContainsEntirePhrase;
                    break;
                }
                else if (mode == "3")
                {
                    searchMethod = TextSearchMethod.ContainsAllTokens;
                    break;
                }
                else if (mode == "4")
                {
                    searchMethod = TextSearchMethod.ContainsAnyToken;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid value.");
                }
            }

            while (string.IsNullOrWhiteSpace(searchValue))
            {
                Console.WriteLine($"Search value for '{parameterName}':");
                searchValue = Console.ReadLine();
            }

            return new TextSearchParameter
            {
                SearchMethod = searchMethod,
                Value = searchValue,
            };
        }

        private static async Task SearchAsync(ProductSearchRepository repo)
        {
            var searchRequest = new ProductSearchModel
            {
                Title = GetSearchParameterFromConsole("Title"),
                Category = GetSearchParameterFromConsole("Category"),
            };

            var searchResults = await repo.SearchAsync(searchRequest);

            Console.WriteLine("Search results:");
            Console.WriteLine("----------------------------------------------------------------------------------");
            foreach (var hit in searchResults)
            {
                var json = JsonConvert.SerializeObject(hit, Formatting.Indented);

                Console.WriteLine(json);
                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------------------------------");
                Console.WriteLine();
            }
        }

        private static async Task SearchRtx3060TiCardsAsync(ElasticClient cli)
        {
            var rtx3060TiCards = await cli.SearchAsync<Product>(search =>
            {
                return search
                    .Query(q =>
                    {
                        var titleQuery = q.MatchPhrase(mp => mp.Field(p => p.Title).Query("ASUS TUF Gaming GeForce RTX 3060 Ti 8GB GDDR6"));
                        var gpuTypeQuery = q.MatchPhrase(mp => mp.Field(p => p.Properties["GPU type"]).Query("NVidia GeForce RTX 3060 Ti"));

                        return titleQuery & gpuTypeQuery;
                    });
            });

            Console.WriteLine("RTX 3060 Ti cards:");
            Console.WriteLine("----------------------------------------------------------------------------------");
            foreach (var hit in rtx3060TiCards.Documents)
            {
                var json = JsonConvert.SerializeObject(hit, Formatting.Indented);

                Console.WriteLine(json);
                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------------------------------");
                Console.WriteLine();
            }
        }

        private static async Task SearchGDDR6CardsAsync(ElasticClient cli)
        {
            var gddr6Cards = await cli.SearchAsync<Product>(search =>
            {
                return search
                    .Query(q =>
                    {
                        var memoryTypeQuery = q.MatchPhrase(mp => mp.Field(p => p.Properties["Memory type"]).Query("GDDR6"));

                        return memoryTypeQuery;
                    })
                    .Aggregations(a =>
                    {
                        return a.Filter("sd", fa => fa.Filter(f => f.MatchPhrase(mp => mp.Field(p => p.Properties["Memory type"]).Query("GDDR6"))));
                    });
            });

            Console.WriteLine("GDDR6 cards:");
            Console.WriteLine("----------------------------------------------------------------------------------");
            foreach (var hit in gddr6Cards.Documents)
            {
                var json = JsonConvert.SerializeObject(hit, Formatting.Indented);

                Console.WriteLine(json);
                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------------------------------");
                Console.WriteLine();
            }
        }

        /*
        private static async Task SearchProductsAsync(ProductSearchRepository repo)
        {
            var results = await repo.SearchAsync(new ProductSearchModel
            {
                Title = new TextSearchParameter<string>
                {
                    Exact = true,
                    Value = "ASUS TUF Gaming GeForce RTX 3060 Ti 8GB GDDR6",
                },
                Category = new TextSearchParameter<string>
                {
                    Exact = true,
                    Value = "Graphics cards",
                },
                IsActive = new SearchParameter<bool>
                {
                    Value = true,
                },
                Page = 1,
                PageSize = 100,
            });

            Console.WriteLine("Results:");
            Console.WriteLine("----------------------------------------------------------------------------------");
            foreach (var hit in results)
            {
                var json = JsonConvert.SerializeObject(hit, Formatting.Indented);

                Console.WriteLine(json);
                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------------------------------");
                Console.WriteLine();
            }
        }
        */
    }
}