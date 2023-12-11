using Microsoft.EntityFrameworkCore;

namespace AspNetCore.ExpressionBuilder.Extensions.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; } = null!;
}
