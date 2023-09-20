using AspNetCore.OData.Models;

namespace AspNetCore.OData.Data
{
    public class DbInitializer
    {

        public static void Initialize(MyWorldDbContext context)
        {

            var Backpack = new Backpack
            {
                Description = Faker.Finance.Ticker()
            };

            var Factions = new List<Faction>
            {
                new Faction
                {
                    Name = Faker.Finance.Ticker()
                },
                new Faction
                {
                    Name = Faker.Finance.Ticker()
                }
            };

            var Weapons = new List<Weapon>
            {
                new Weapon
                {
                    Name = Faker.Company.Name()
                },
                new Weapon
                {
                    Name = Faker.Company.Name()
                }
            };

            var emp = new List<Employee>
            {
                    new Employee { Id = 1, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name(), Backpack =Backpack, Factions = Factions, Weapons = Weapons },
                    new Employee { Id = 2, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 3, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 4, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 5, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 6, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 7, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 8, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 9, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 10, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 11, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 12, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 13, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 14, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()},
                    new Employee { Id = 15, FirstName = Faker.Name.First(), LastName = Faker.Name.Last(), Salary = Faker.RandomNumber.Next(), JobRole = Faker.Currency.Name()}
            };

            if (!context.Employee.Any())
            {
                context.Employee.AddRange(emp);
                context.SaveChanges();
            }

            //context.Employee.RemoveRange(context.Employee);
            //context.SaveChanges();

        }
    }
}
