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
                //b.Property(_=>_.Status).HasConversion<string>().HasMaxLength(20);
                //b.Property(_=>_.Status).HasConversion<string>(); // cách 2 convert enum sang string  khi lưu lên db
                //b.Property(_ => _.Status).HasConversion(new EnumToStringConverter<OrderStatus>()); // cách 3 convert enum sang string  khi lưu lên db

                // cấu hình AutoInclude sẽ không có ghi vào history migration, cho nên commit thì không có gì thay đổi cả
                b.Navigation(_ => _.Weapons).AutoInclude();
                b.Navigation(_ => _.Backpack).AutoInclude();
                b.Navigation(_ => _.Factions).AutoInclude();

                #region one to many
                //// nếu trong class Father có cấu hình ICollection<Child> và trong class Child có cấu hình object Father
                //// thì nên áp dụng c1 -> không nên áp dụng c2 hoặc c3 vì sẽ không include được ICollection<Child>
                //b.HasMany(_ => _.Weapons).WithOne(_ => _.Nemo); // c1

                //// nếu trong class Father không cấu hình ICollection<Child> và trong class Child có cấu hình object Father
                //// thì nên áp dụng c2  -> c1 và c3 không áp dụng được
                //b.HasMany<Weapon>().WithOne(_ => _.Nemo); // c2

                //// nếu trong class Father không cấu hình ICollection<Child> và trong class Child không cấu hình object Father
                //// thì áp dụng c3
                //b.HasMany<Weapon>().WithOne(); // c3

                // lưu ý: trong class Child tên property FK phải là tên class + Id, ví dụ: class Father thì tên property FK là FatherId hoặc PropertiesFatherId -> vào class Backpack sẽ rõ
                // nếu không đúng cấu trúc thì kể cả không cấu hình relateionship trong OnModelCreating mà để ef tự tạo hay có cấu hình relateionship mà không có HasForeignKey thì nó sẽ tự động tạo ra FK với tên là tên class + Id
                // nếu c1 và c2 mà tên property FK mà không đúng cấu trúc ClassFatherId hoặc PropertiesFatherId
                // thì có thể áp dung [ForeignKey("NemoId")] trên object Father trong class Child -> vào class Backpack sẽ rõ
                // hoặc cấu hình .HasForeignKey(_ => _.NemoId); 
                // nếu c3 mà tên property FK mà không đúng cấu trúc ClassFatherId (không có PropertiesFatherId vì nó không cấu hình object Father )
                // thì chỉ có thể áp dụng .HasForeignKey(_ => _.NemoId); như bên dưới 

                //b.HasMany(_ => _.Weapons).WithOne(_ => _.Nemo).HasForeignKey(_ => _.CharacterId);
                //b.HasMany<Weapon>().WithOne(_ => _.Nemo).HasForeignKey(_ => _.CharacterId);
                //b.HasMany<Weapon>().WithOne().HasForeignKey(_ => _.CharacterId);

                #endregion

                #region one to one
                //b.HasOne(_ => _.Backpack).WithOne(_ => _.Nemo).HasForeignKey<Backpack>(_ => _.NemoId);
                //b.HasOne<Backpack>().WithOne().HasForeignKey<Backpack>(_ => _.NemoId);
                #endregion

                #region  many to many
                //b.HasMany<Faction>().WithMany().UsingEntity(_ => _.ToTable("CharacterFaction"));
                //b.HasMany(_ => _.Factions).WithMany(_ => _.Characters)
                //.UsingEntity(_ =>
                //{
                //    _.ToTable("CharacterFaction");
                //    _.Property("CharactersId").HasColumnName("CharacterId");
                //    _.Property("FactionsId").HasColumnName("FactionId");
                //}); 
                #endregion

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
