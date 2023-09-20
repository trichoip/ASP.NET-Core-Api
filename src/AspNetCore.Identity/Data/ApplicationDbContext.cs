using AspNetCore.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Identity.Data
{
    // mặc định là IdentityDbContext<IdentityUser, IdentityRole, string> key là string
    // nếu để IdentityDbContext và IdentityDbContext<IdentityUser> thì vẫn giống như trên
    // IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid> key là Guid nếu đổi key phải xóa table tạo lại từ đầu
    // nếu đổi key thì phải cấu hình đầy đủ cả 3 như hàng trên
    // có thể custom lại IdentityUser hay Identity class khác thì kế thừa là được
    // nếu không cấu hình key thì IdentityDbContext<ApplicationUser>
    // còn nếu đổi key thì phải IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid> (ApplicationUser:IdentityUser<Guid>)
    // IdentityDbContext<ApplicationUser, IdentityRole<int>, int> (ApplicationUser:IdentityUser<int>)
    // f12 IdentityDbContext để hiểu rõ hơn
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {

        public DbSet<Backpack> Backpacks { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Faction> Factions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // muốn sữa cái gì bắt buộc phải gọi hàm này trước

            // Bỏ tiền tố AspNet của các bảng: mặc định các bảng trong IdentityDbContext có
            // tên với tiền tố AspNet như: AspNetUserRoles, AspNetUser ...
            // Đoạn mã sau chạy khi khởi tạo DbContext, tạo database sẽ loại bỏ tiền tố đó
            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    var tableName = entityType.GetTableName();
            //    if (tableName.StartsWith("AspNet"))
            //    {
            //        entityType.SetTableName(tableName.Substring(6));
            //    }
            //}

            modelBuilder.Entity("ApplicationUserFaction", b =>
            {
                b.Property("FactionsId").HasColumnName("FactionId");

                b.Property("UsersId").HasColumnName("UserId");

                b.ToTable("UserFactions");
            });

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.Property(e => e.Email).HasColumnName("EMail").HasMaxLength(128);
                b.Property(e => e.CustomTag).HasMaxLength(128);

                b.ToTable("Users");
            });

            modelBuilder.Entity<IdentityUserClaim<Guid>>(b =>
            {
                b.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<Guid>>(b =>
            {
                b.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityUserToken<Guid>>(b =>
            {
                b.ToTable("UserTokens");
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.Property(e => e.Description).HasMaxLength(255);

                b.ToTable("Roles");
            });

            modelBuilder.Entity<IdentityRoleClaim<Guid>>(b =>
            {
                b.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>(b =>
            {
                b.ToTable("UserRoles");
            });
        }
    }
}