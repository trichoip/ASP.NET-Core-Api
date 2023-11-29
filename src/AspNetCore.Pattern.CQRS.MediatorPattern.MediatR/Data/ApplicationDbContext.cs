using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
}