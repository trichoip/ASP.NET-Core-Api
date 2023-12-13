using AspNetCore.Database.EntityFramework.Data;
using AspNetCore.Database.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AspNetCore.Database.EntityFramework.SeedData;

public class StoreContextSeed
{
    public static async Task SeedAsync(DataContext storeContext, ILoggerFactory loggerFactory)
    {
        try
        {
            if (!await storeContext.Characters.AnyAsync())
            {
                var character = File.ReadAllText("../AspNetCore.EntityFramework/SeedData/character.json");
                var characters = JsonSerializer.Deserialize<List<Character>>(character);
                foreach (var brandItem in characters)
                {
                    await storeContext.Characters.AddAsync(brandItem);
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
