using AspNetCore.Extensions.Entities;
using System.Security.Claims;

namespace AspNetCore.Extensions.Services
{
    public interface IRoleManager
    {

        IQueryable<Role> Roles { get; }
        Task<Role> CreateAsync(Role role);
        Task<Role> UpdateAsync(Role role);
        Task DeleteAsync(Role role);
        Task<bool> RoleExistsAsync(string roleName);
        Task<Role?> FindByIdAsync(Guid roleId);
        Task<Role?> FindByNameAsync(string roleName);
        Task<Role> AddClaimAsync(Role role, Claim claim);
        Task<Role> RemoveClaimAsync(Role role, Claim claim);
        Task<IList<Claim>> GetClaimsAsync(Role role);
        Task ValidateRoleAsync(Role role);

    }
}
