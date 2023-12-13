using AspNetCore.Database.Mysql.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Database.Mysql.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Character> Characters { get; set; }
    public DbSet<Backpack> Backpacks { get; set; }
    public DbSet<Weapon> Weapons { get; set; }
    public DbSet<Faction> Factions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>(b =>
        {
            b.Property(_ => _.Name)
                .HasColumnType("nvarchar(20)");

            b.Navigation(_ => _.Weapons).AutoInclude();
            b.Navigation(_ => _.Backpack).AutoInclude();
            b.Navigation(_ => _.Factions).AutoInclude();

        });

        modelBuilder.Entity<Backpack>(b =>
        {
            b.Property(_ => _.Description)
                .HasColumnType("nvarchar(20)");
        });

        modelBuilder.Entity<Faction>(b =>
        {
            b.Property(_ => _.Name)
                .HasColumnType("nvarchar(20)");
        });

        modelBuilder.Entity<Weapon>(b =>
        {
            b.Property(_ => _.Name)
                .HasColumnType("nvarchar(20)");
        });

    }
}
