using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Repository.Data.Context
{
    public static class StoreDbContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context)
        {
            if (!context.ProductBrands.Any())
            {
                var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                if (brands?.Count > 0)
                {
                    foreach (var brand in brands)
                        await context.ProductBrands.AddAsync(brand);

                    await context.SaveChangesAsync();
                }
            }


            if (!context.ProductTypes.Any())
            {
                var typesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if (types?.Count > 0)
                {
                    foreach (var type in types)
                        await context.ProductTypes.AddAsync(type);

                    await context.SaveChangesAsync();
                }
            }


            if (!context.Products.Any())
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (products?.Count > 0)
                {
                    foreach (var product in products)
                        await context.Products.AddAsync(product);

                    await context.SaveChangesAsync();
                }
            }


            if(!context.DeliveryMethods.Any())
            {
                var MethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(MethodData);

                foreach (var method in DeliveryMethods)
                    await context.DeliveryMethods.AddAsync(method);    

                await context.SaveChangesAsync();
            }
        }
    }
}
