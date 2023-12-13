using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;
using System.Text.Json;

namespace AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Data.SeedData;

public class StoreContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext storeContext, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!storeContext.Clubs.Any())
            {
                var branddata = File.ReadAllText("../Skinet.Infrastracture/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<Club>>(branddata);
                foreach (var brandItem in brands)
                {
                    await storeContext.Clubs.AddAsync(brandItem);
                }
                await storeContext.SaveChangesAsync();
            }

            if (!storeContext.Players.Any())
            {
                var productsdata = File.ReadAllText("../Skinet.Infrastracture/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Player>>(productsdata);

                foreach (var productsItems in products)
                {
                    await storeContext.Players.AddAsync(productsItems);
                }
                await storeContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger<StoreContextSeed>();
            logger.LogError(ex, "Something went wrong with your request");
        }
    }
}
