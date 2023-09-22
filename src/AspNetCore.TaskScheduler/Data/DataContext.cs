using AspNetCore.TaskScheduler.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.TaskScheduler.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Person> People => Set<Person>();
    }
}
