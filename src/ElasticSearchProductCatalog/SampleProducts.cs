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
                    new ProductProperty { Key = "Memory size", Value = "8 GB" },
                    new ProductProperty { Key = "Memory bandwidth", Value = "256 bit" },
                    new ProductProperty { Key = "Memory type", Value = "GDDR6" },
                    new ProductProperty { Key = "Board manufacturer", Value = "ASUS" },
                    new ProductProperty { Key = "Interface", Value = "PCI Express 4.0" },
                    new ProductProperty { Key = "GPU type", Value = "NVidia GeForce RTX 3060 Ti" },
                    new ProductProperty { Key = "GPU boost clock", Value = "1695 MHz" },
                    new ProductProperty { Key = "CUDA cores", Value = "4864" },
                    new ProductProperty { Key = "HDMI 2.1 ports", Value = "2" },
                    new ProductProperty { Key = "DisplayPort 1.4a ports", Value = "3" },
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
                    new ProductProperty { Key = "Memory size", Value = "12 GB" },
                    new ProductProperty { Key = "Memory bandwidth", Value = "192 bit" },
                    new ProductProperty { Key = "Memory type", Value = "GDDR6" },
                    new ProductProperty { Key = "Board manufacturer", Value = "EVGA" },
                    new ProductProperty { Key = "Interface", Value = "PCI Express 4.0" },
                    new ProductProperty { Key = "GPU type", Value = "NVidia GeForce RTX 3060" },
                    new ProductProperty { Key = "GPU boost clock", Value = "1882 MHz" },
                    new ProductProperty { Key = "CUDA cores", Value = " 3584" },
                    new ProductProperty { Key = "HDMI 2.1 ports", Value = "2" },
                    new ProductProperty { Key = "DisplayPort 1.4a ports", Value = "3" },
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
                    new ProductProperty { Key = "Cores", Value = "6" },
                    new ProductProperty { Key = "Threads", Value = "12" },
                    new ProductProperty { Key = "Clock speed", Value = "4.6 GHz" },
                    new ProductProperty { Key = "Socket", Value = "AM4" },
                    new ProductProperty { Key = "TDP", Value = "65 W" },
                },
            };

            yield return new Product
            {
                Category = "CPUs",
                Id = "e752547c-c2de-4b39-9237-8e481e3a757c",
                IsActive = true,
                Price = 339.99m,
                Title = "Intel Core i7-11700",
                Properties =
                {
                    new ProductProperty { Key = "Cores", Value = "8" },
                    new ProductProperty { Key = "Threads", Value = "16" },
                    new ProductProperty { Key = "Clock speed", Value = "4.9 GHz" },
                    new ProductProperty { Key = "Socket", Value = "LGA 1200" },
                    new ProductProperty { Key = "TDP", Value = "65 W" },
                },
            };
        }
    }
}