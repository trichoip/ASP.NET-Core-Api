using AspNetCore.Api.Extensions.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Api.Extensions.Data;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; } = default!;
    public virtual DbSet<UserClaim> UserClaims { get; set; } = default!;
    public virtual DbSet<UserLogin> UserLogins { get; set; } = default!;
    public virtual DbSet<UserToken> UserTokens { get; set; } = default!;
    public virtual DbSet<UserRole> UserRoles { get; set; } = default!;

    public virtual DbSet<Role> Roles { get; set; } = default!;
    public virtual DbSet<RoleClaim> RoleClaims { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<User>(b =>
        {
            b.ToTable("AspNetUsers");

            b.HasKey(u => u.Id);

            b.HasIndex(u => u.NormalizedUserName).HasDatabaseName("UserNameIndex").IsUnique();
            b.HasIndex(u => u.NormalizedEmail).HasDatabaseName("EmailIndex");

            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
            b.Property(u => u.UserName).HasMaxLength(256);
            b.Property(u => u.NormalizedUserName).HasMaxLength(256);
            b.Property(u => u.Email).HasMaxLength(256);
            b.Property(u => u.NormalizedEmail).HasMaxLength(256);

            b.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            b.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            b.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            b.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
        });

        builder.Entity<UserClaim>(b =>
        {
            b.ToTable("AspNetUserClaims");

            b.HasKey(uc => uc.Id);

        });

        builder.Entity<UserLogin>(b =>
        {
            b.ToTable("AspNetUserLogins");

            b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

            b.Property(l => l.LoginProvider).HasMaxLength(256);
            b.Property(l => l.ProviderKey).HasMaxLength(256);

        });

        builder.Entity<UserToken>(b =>
        {
            b.ToTable("AspNetUserTokens");

            b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

            b.Property(t => t.LoginProvider).HasMaxLength(256);
            b.Property(t => t.Name).HasMaxLength(256);

        });

        builder.Entity<Role>(b =>
        {
            b.ToTable("AspNetRoles");

            b.HasKey(r => r.Id);

            b.HasIndex(r => r.NormalizedName).HasDatabaseName("RoleNameIndex").IsUnique();

            b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();
            b.Property(u => u.Name).HasMaxLength(256);
            b.Property(u => u.NormalizedName).HasMaxLength(256);

            b.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            b.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
        });

        builder.Entity<RoleClaim>(b =>
        {
            b.ToTable("AspNetRoleClaims");

            b.HasKey(rc => rc.Id);

        });

        builder.Entity<UserRole>(b =>
        {
            b.ToTable("AspNetUserRoles");

            b.HasKey(r => new { r.UserId, r.RoleId });

        });

    }
}
