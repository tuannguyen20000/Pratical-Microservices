using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Product.API.Entities;
using ILogger = Serilog.ILogger;

namespace Product.API.Persistence
{
    public class ProductContextSeed
    {
        public static async Task SeedProductAsync(ProductContext productContext, ILogger logger)
        {
            if (!productContext.Products.Any())
            {
                productContext.AddRange(getCatalogProducts());
                await productContext.SaveChangesAsync();
                logger.Information("Seeded data for Product Db associated with context {DbContextName}", nameof(ProductContext));
            }
        }

        private static IEnumerable<CatalogProduct> getCatalogProducts()
        {
            return new List<CatalogProduct>{
                new (){
                    No = "No1",
                    Name = "Giay",
                    Description = "Giay dep",
                    Price = (decimal)20000.00
                },
                new (){
                    No = "No2",
                    Name = "Dep",
                    Description = "Dep dep",
                    Price = (decimal)13000.00
                }
            };
        }
    }
}