using System;
using System.Threading.Tasks;
using ElasticSearchProductCatalog.Models;
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

        private static async Task SearchAsync(ProductSearchRepository repo)
        {
            var searchRequest = new ProductSearchModel
            {
                Title = GetTextSearchParameterFromConsole("Title"),
                Category = GetTextSearchParameterFromConsole("Category"),
                IsActive = GetBoolSearchParameterFromConsole("Is active"),
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

        private static TextSearchParameter GetTextSearchParameterFromConsole(string parameterName)
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
                    Console.WriteLine("Invalid value. Press any key to try again.");
                    Console.ReadKey();
                    Console.Clear();
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

        private static SearchParameter<bool> GetBoolSearchParameterFromConsole(string parameterName)
        {
            while (true)
            {
                Console.WriteLine($"Search mode for '{parameterName}':");
                Console.WriteLine($"0 - Don't search for {parameterName}");
                Console.WriteLine($"1 - True");
                Console.WriteLine($"2 - False");

                var mode = Console.ReadLine().Trim();
                if (mode == "0")
                {
                    return null;
                }

                if (mode == "1")
                {
                    return new SearchParameter<bool> { Value = true };
                }
                else if (mode == "2")
                {
                    return new SearchParameter<bool> { Value = false };
                }
                else
                {
                    Console.WriteLine("Invalid value. Press any key to try again.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}