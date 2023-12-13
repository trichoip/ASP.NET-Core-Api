using AspNetCore.Api.Extensions.Entities;
using System.Security.Claims;

namespace AspNetCore.Api.Extensions.Repositories;

public interface IRoleStore
{
    Task SaveChanges(CancellationToken cancellationToken);
    IQueryable<Role> Roles { get; }
    Task<Role> CreateAsync(Role role, CancellationToken cancellationToken);
    Task<Role> UpdateAsync(Role role, CancellationToken cancellationToken);
    Task DeleteAsync(Role role, CancellationToken cancellationToken);
    Task<Role?> FindByIdAsync(Guid roleId, CancellationToken cancellationToken);
    Task<Role?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken);
    Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default);
    Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default);
    Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default);

}
