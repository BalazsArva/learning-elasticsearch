using System.Collections.Generic;
using ElasticSearchProductCatalog.Models;

namespace ElasticSearchProductCatalog
{
    public static class SampleProducts
    {
        public static IEnumerable<Product> GetProducts()
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

            yield return new Product
            {
                Category = "CPUs",
                Id = "e3b01472-5364-4ab1-b818-2c801f2030dd",
                IsActive = true,
                Price = 272.99m,
                Title = "AMD Ryzen 5 5600x",
                Properties =
                {
                    ["Cores"] = "6",
                    ["Threads"] = "12",
                    ["Clock speed"] = "4.6 GHz",
                    ["Socket"] = "AM4",
                    ["TDP"] = "65 W",
                },
            };
        }
    }
}