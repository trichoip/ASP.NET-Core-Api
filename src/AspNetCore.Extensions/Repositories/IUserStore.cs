using AspNetCore.Extensions.DTOs;
using AspNetCore.Extensions.Entities;
using System.Security.Claims;

namespace AspNetCore.Extensions.Repositories
{
    public interface IUserStore
    {
        Task SaveChanges(CancellationToken cancellationToken);
        IQueryable<User> Users { get; }
        Task<User> CreateAsync(User user, CancellationToken cancellationToken);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken);
        Task DeleteAsync(User user, CancellationToken cancellationToken);
        Task<User?> FindByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
        Task<Role?> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken);
        Task<UserRole?> FindUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken);
        Task<User?> FindUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserLogin?> FindUserLoginAsync(Guid userId, string loginProvider, string providerKey, CancellationToken cancellationToken);
        Task<UserLogin?> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);
        Task AddToRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken);
        Task RemoveFromRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken);
        Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken);
        Task<bool> IsInRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken);
        Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken);
        Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken);
        Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken);
        Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken);
        Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken);
        Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken);
        Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken);
        Task<User?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);
        Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
        Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken);
        Task<IList<User>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken);
        Task<UserToken?> FindTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken);
        Task AddUserTokenAsync(UserToken token);
        Task RemoveUserTokenAsync(UserToken token);
        Task RemoveTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken);
        Task<string?> GetTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken);
        Task SetTokenAsync(User user, string loginProvider, string name, string? value, CancellationToken cancellationToken);
        Task<int> CountCodesAsync(User user, CancellationToken cancellationToken);
        Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken);
        Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken);
        Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken);
        Task<string?> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken);

    }
}
