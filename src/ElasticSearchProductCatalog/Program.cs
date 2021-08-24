using System;
using System.Collections.Generic;
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

            foreach (var product in GetProducts())
            {
                await repo.InsertAsync(product);
            }

            await SearchProductsAsync(repo);

            Console.WriteLine("All done.");
            Console.ReadKey();
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

        private static IEnumerable<Product> GetProducts()
        {
            yield return new Product
            {
                Category = "Graphics cards",
                Id = "2ea9c080-4519-4c4d-a79a-b85941f2b7cb",
                IsActive = true,
                Price = 399.99m,
                Title = "ASUS TUF Gaming GeForce RTX 3060 Ti 8GB GDDR6",
                Properties =
                {
                    ["Memory size"] = "8 GB",
                    ["Memory bandwidth"] = "256 bit",
                    ["Memory type"] = "GDDR6",
                    ["Board manufacturer"] = "ASUS",
                    ["Interface"] = "PCI Express 4.0",
                    ["GPU type"] = "NVidia GeForce RTX 3060 Ti",
                    ["GPU boost clock"] = "1695 MHz",
                    ["CUDA cores"] = "4864",
                    ["HDMI 2.1 ports"] = "2",
                    ["DisplayPort 1.4a ports"] = "3",
                }
            };

            yield return new Product
            {
                Category = "Graphics cards",
                Id = "fba3ff4d-fe7a-4f2a-b56f-d9b15876b603",
                IsActive = true,
                Price = 379.99m,
                Title = "EVGA GeForce RTX 3060 12GB XC Gaming",
                Properties =
                {
                    ["Memory size"] = "12 GB",
                    ["Memory bandwidth"] = "192 bit",
                    ["Memory type"] = "GDDR6",
                    ["Board manufacturer"] = "EVGA",
                    ["Interface"] = "PCI Express 4.0",
                    ["GPU type"] = "NVidia GeForce RTX 3060",
                    ["GPU boost clock"] = "1882 MHz",
                    ["CUDA cores"] = " 3584",
                    ["HDMI 2.1 ports"] = "2",
                    ["DisplayPort 1.4a ports"] = "3",
                }
            };
        }
    }
}