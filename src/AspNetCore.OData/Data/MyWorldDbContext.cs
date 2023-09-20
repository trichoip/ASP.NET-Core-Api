using AspNetCore.OData.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.OData.Data
{
    public class MyWorldDbContext : DbContext
    {
        public MyWorldDbContext(DbContextOptions<MyWorldDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Weapon> Weapon { get; set; }
        public DbSet<Faction> Faction { get; set; }
        public DbSet<Backpack> Backpack { get; set; }

    }
}
