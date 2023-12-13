using AspNetCore.TaskScheduler.Hangfire.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.TaskScheduler.Hangfire.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Person> People => Set<Person>();
}
