using AspNetCore.Database.EntityFramework.Data;
using AspNetCore.Database.EntityFramework.Entities;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Database.EntityFramework.SeedData;

public class DbInitializer
{
    public static async Task Initialize(DataContext context)
    {

        var Backpack = new Faker<Backpack>()
                            .RuleFor(c => c.Description, f => f.Name.JobTitle());

        var Factions = new Faker<Faction>()
                            .RuleFor(c => c.Name, f => f.Name.JobTitle());

        var Weapons = new Faker<Weapon>()
                            .RuleFor(c => c.Name, f => f.Name.JobTitle());

        var Characters = new Faker<Character>()
                            .RuleFor(c => c.Name, f => f.Person.UserName)
                            .RuleFor(c => c.Backpack, Backpack.Generate())
                            .RuleFor(c => c.Factions, Factions.Generate(2))
                            .RuleFor(c => c.Weapons, Weapons.Generate(2))
                            .Generate(2);

        if (!await context.Characters.AnyAsync())
        {
            await context.Characters.AddRangeAsync(Characters);

            await context.SaveChangesAsync();
        }

    }
}
