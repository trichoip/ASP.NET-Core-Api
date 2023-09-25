using AspNetCore.EntityFramework.Data;
using AspNetCore.EntityFramework.Models;
using Bogus;

namespace AspNetCore.EntityFramework.DataSeeding
{
    public class DbInitializer
    {
        public static void Initialize(DataContext context)
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
                                .RuleFor(c => c.Factions, Factions.Generate(20))
                                .RuleFor(c => c.Weapons, Weapons.Generate(10))
                                .Generate(2);

            if (!context.Characters.Any())
            {
                context.Characters.AddRange(Characters);

                context.SaveChanges();
            }

        }
    }
}
