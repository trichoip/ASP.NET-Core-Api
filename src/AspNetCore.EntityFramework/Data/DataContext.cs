using AspNetCore.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.EntityFramework.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Backpack> Backpacks { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Faction> Factions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // cần sữa cái nào thì cấu hình cái đó
            modelBuilder.Entity<Character>(b =>
            {
                b.Property(_ => _.Name).HasColumnType("nvarchar(200)");

                // cấu hình AutoInclude sẽ không có ghi vào history migration, cho nên commit thì không có gì thay đổi cả
                b.Navigation(_ => _.Weapons).AutoInclude();
                b.Navigation(_ => _.Backpack).AutoInclude();
                b.Navigation(_ => _.Factions).AutoInclude();

            });

            modelBuilder.Entity<Backpack>(b =>
            {
                b.Property(_ => _.Description).HasColumnType("nvarchar(200)");
            });

            modelBuilder.Entity<Faction>(b =>
            {
                b.Property(_ => _.Name).HasColumnType("nvarchar(200)");
            });

            modelBuilder.Entity<Weapon>(b =>
            {
                b.Property(_ => _.Name).HasColumnType("nvarchar(200)");
            });

            modelBuilder.Entity("CharacterFaction", b =>
            {
                b.Property("CharactersId").HasColumnName("CharacterId");

                b.Property("FactionsId").HasColumnName("FactionId");

                b.ToTable("CharacterFaction");
            });

        }
    }
}
