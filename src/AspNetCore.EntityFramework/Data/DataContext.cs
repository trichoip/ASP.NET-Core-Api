using AspNetCore.EntityFramework.Entities;
using AspNetCore.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AspNetCore.EntityFramework.Data
{
    public class DataContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICurrentUserService _currentUserService;

        public DataContext(DbContextOptions<DataContext> options,
            IHttpContextAccessor httpContextAccessor,
            ICurrentUserService currentUserService) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _currentUserService = currentUserService;
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Backpack> Backpacks { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Faction> Factions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // giúp lấy được các cấu hình trong các class : IEntityTypeConfiguration mà không cần phải cấu hình vào OnModelCreating
            // nếu không có thì phải thêm cấu hình như -> modelBuilder.ApplyConfiguration(new CharacterConfiguration());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // cần sữa cái nào thì cấu hình cái đó
            modelBuilder.Entity<Character>(b =>
            {
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

        #region Cau Hinh Auditable trong class Interceptors nên không cần cấu hình ở đây
        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var entries = ChangeTracker
        //     .Entries()
        //     .Where(e => e.Entity is BaseAuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
        //    foreach (var entityEntry in entries)
        //    {
        //        if (entityEntry.State == EntityState.Added)
        //        {
        //            ((BaseAuditableEntity)entityEntry.Entity).CreatedAt = DateTimeOffset.UtcNow;
        //            ((BaseAuditableEntity)entityEntry.Entity).CreatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Anonymous";
        //        }
        //        else
        //        {
        //            Entry((BaseAuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
        //            Entry((BaseAuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
        //        }
        //        ((BaseAuditableEntity)entityEntry.Entity).ModifiedAt = DateTimeOffset.UtcNow;
        //        ((BaseAuditableEntity)entityEntry.Entity).ModifiedBy = _currentUserService.Id;
        //    }
        //    return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false); ;
        //}
        //public override int SaveChanges()
        //{
        //    return SaveChangesAsync().GetAwaiter().GetResult();
        //}
        #endregion
    }
}
