using AspNetCore.Extensions.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Extensions.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> UsersSet { get { return Set<User>(); } }
        public virtual DbSet<Role> Roles { get { return Set<Role>(); } }
        public virtual DbSet<RoleClaim> RoleClaims { get { return Set<RoleClaim>(); } }
        public virtual DbSet<UserClaim> UserClaims { get { return Set<UserClaim>(); } }
        public virtual DbSet<UserRole> UserRoles { get { return Set<UserRole>(); } }
        public virtual DbSet<UserLogin> UserLogins { get { return Set<UserLogin>(); } }
        public virtual DbSet<UserToken> UserTokens { get { return Set<UserToken>(); } }
    }
}
