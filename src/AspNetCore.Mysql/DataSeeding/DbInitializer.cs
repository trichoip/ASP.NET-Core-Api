using AspNetCore.Mysql.Data;
using AspNetCore.Mysql.Models;

namespace AspNetCore.Mysql.DataSeeding
{
    public class DbInitializer
    {
        public static void Initialize(DataContext context)
        {

            var Backpack = new Backpack
            {
                Description = "Test Backpack"
            };

            var Factions = new List<Faction>
            {
                new Faction
                {
                    Name = "Test Faction 1"
                },
                new Faction
                {
                    Name = "Test Faction 2"
                }
            };

            var Weapons = new List<Weapon>
            {
                new Weapon
                {
                    Name = "Test Weapon 1"
                },
                new Weapon
                {
                    Name = "Test Weapon 2"
                }
            };

            if (!context.Characters.Any())
            {
                context.Characters.Add(new Character
                {
                    Name = "Test Character",
                    Backpack = Backpack,
                    Factions = Factions,
                    Weapons = Weapons

                });

                context.SaveChanges();
            }

        }
    }
}
