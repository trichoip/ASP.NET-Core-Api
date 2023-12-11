using Microsoft.EntityFrameworkCore;

namespace AspNetCore.ExpressionBuilder.LinqKit.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;
    public DbSet<Pop> Pops { get; set; } = null!;
}
