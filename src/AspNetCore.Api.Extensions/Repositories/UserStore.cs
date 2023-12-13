using AspNetCore.Api.Extensions.Data;
using AspNetCore.Api.Extensions.DTOs;
using AspNetCore.Api.Extensions.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace AspNetCore.Api.Extensions.Repositories;

public class UserStore : IUserStore
{

    private readonly ApplicationDbContext Context;

    public bool AutoSaveChanges { get; set; } = true;

    private const string InternalLoginProvider = "[AspNetUserStore]";
    private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
    private const string RecoveryCodeTokenName = "RecoveryCodes";

    public UserStore(ApplicationDbContext context)
    {
        Context = context;
    }

    public IQueryable<User> Users { get { return Context.Users; } }

    public Task SaveChanges(CancellationToken cancellationToken)
    {
        return AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        Context.Add(user);
        await SaveChanges(cancellationToken);

        return user;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        Context.Attach(user);
        user.ConcurrencyStamp = Guid.NewGuid().ToString();
        Context.Update(user);
        try
        {
            await SaveChanges(cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new DbUpdateConcurrencyException("Optimistic concurrency failure", exception);
        }
        catch (Exception exception)
        {
            throw new Exception("Error updating user.", exception);
        }

        return user;
    }

    public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        Context.Remove(user);
        try
        {
            await SaveChanges(cancellationToken);
        }
        catch (DbUpdateConcurrencyException exception)
        {
            throw new DbUpdateConcurrencyException("Optimistic concurrency failure", exception);
        }
        catch (Exception exception)
        {
            throw new Exception("Error Delete user.", exception);
        }
    }

    public Task<User?> FindByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Context.Users.FindAsync(new object?[] { userId }, cancellationToken).AsTask();
    }

    public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Context.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName, cancellationToken);
    }

    public Task<Role?> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        return Context.Roles.SingleOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
    }

    public Task<UserRole?> FindUserRoleAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        return Context.UserRoles.FindAsync(new object[] { userId, roleId }, cancellationToken).AsTask();
    }

    public Task<User?> FindUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return Users.SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);
    }

    public Task<UserLogin?> FindUserLoginAsync(Guid userId, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        return Context.UserLogins.SingleOrDefaultAsync(userLogin => userLogin.UserId.Equals(userId) && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);
    }

    public Task<UserLogin?> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        return Context.UserLogins.SingleOrDefaultAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);
    }

    public async Task AddToRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (string.IsNullOrWhiteSpace(normalizedRoleName))
        {
            throw new ArgumentException("Value Cannot Be Null Or Empty", nameof(normalizedRoleName));
        }
        var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
        if (roleEntity == null)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "Role Not Found", normalizedRoleName));
        }

        var userRole = new UserRole { UserId = user.Id, RoleId = roleEntity.Id };
        Context.UserRoles.Add(userRole);
        // lúc gọi hàm này thì gọi luôn hàm updateasyce
        // tường tự với các hàm khác
        //TODO: await SaveChanges(cancellationToken);
    }

    public async Task RemoveFromRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (string.IsNullOrWhiteSpace(normalizedRoleName))
        {
            throw new ArgumentException("Value Cannot Be Null Or Empty", nameof(normalizedRoleName));
        }
        var roleEntity = await FindRoleAsync(normalizedRoleName, cancellationToken);
        if (roleEntity != null)
        {
            var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);
            if (userRole != null)
            {
                Context.UserRoles.Remove(userRole);
                //TODO: await SaveChanges(cancellationToken);
            }
        }
    }

    public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var userId = user.Id;
        var query = from userRole in Context.UserRoles
                    join role in Context.Roles on userRole.RoleId equals role.Id
                    where userRole.UserId.Equals(userId)
                    select role.Name;
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> IsInRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (string.IsNullOrWhiteSpace(normalizedRoleName))
        {
            throw new ArgumentException("Value Cannot Be Null Or Empty", nameof(normalizedRoleName));
        }
        var role = await FindRoleAsync(normalizedRoleName, cancellationToken);
        if (role != null)
        {
            var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
            return userRole != null;
        }
        return false;
    }

    public async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return await Context.UserClaims.Where(uc => uc.UserId.Equals(user.Id)).Select(c => c.ToClaim()).ToListAsync(cancellationToken);
    }

    public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }
        foreach (var claim in claims)
        {
            var userClaim = new UserClaim { UserId = user.Id };
            userClaim.InitializeFromClaim(claim);
            Context.UserClaims.Add(userClaim);
            // TODO: await SaveChanges(cancellationToken);
        }
        return Task.FromResult(false);
    }

    public async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }
        if (newClaim == null)
        {
            throw new ArgumentNullException(nameof(newClaim));
        }

        var matchedClaims = await Context.UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);
        foreach (var matchedClaim in matchedClaims)
        {
            matchedClaim.ClaimValue = newClaim.Value;
            matchedClaim.ClaimType = newClaim.Type;
        }
    }

    public async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }
        foreach (var claim in claims)
        {
            var matchedClaims = await Context.UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync(cancellationToken);
            foreach (var c in matchedClaims)
            {
                Context.UserClaims.Remove(c);
                // TODO: await SaveChanges(cancellationToken);
            }
        }
    }

    public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (login == null)
        {
            throw new ArgumentNullException(nameof(login));
        }

        var userLogin = new UserLogin
        {
            UserId = user.Id,
            ProviderKey = login.ProviderKey,
            LoginProvider = login.LoginProvider,
            ProviderDisplayName = login.ProviderDisplayName
        };

        Context.UserLogins.Add(userLogin);
        // TODO: await SaveChanges(cancellationToken);
        // luu ý  Task.FromResult(false);
        return Task.FromResult(false);
    }

    public async Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var entry = await FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
        if (entry != null)
        {
            Context.UserLogins.Remove(entry);
            // TODO: await SaveChanges(cancellationToken);
        }
    }

    public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var userId = user.Id;
        return await Context.UserLogins.Where(l => l.UserId.Equals(userId))
            .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToListAsync(cancellationToken);
    }

    public async Task<User?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var userLogin = await FindUserLoginAsync(loginProvider, providerKey, cancellationToken);
        if (userLogin != null)
        {
            return await FindUserAsync(userLogin.UserId, cancellationToken);
        }
        return null;
    }

    public Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Users.SingleOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail, cancellationToken);
    }

    public async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }

        var query = from userclaims in Context.UserClaims
                    join user in Users on userclaims.UserId equals user.Id
                    where userclaims.ClaimValue == claim.Value
                    && userclaims.ClaimType == claim.Type
                    select user;

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IList<User>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (string.IsNullOrEmpty(normalizedRoleName))
        {
            throw new ArgumentNullException(nameof(normalizedRoleName));
        }

        var role = await FindRoleAsync(normalizedRoleName, cancellationToken);

        if (role != null)
        {
            var query = from userrole in Context.UserRoles
                        join user in Users on userrole.UserId equals user.Id
                        where userrole.RoleId.Equals(role.Id)
                        select user;

            return await query.ToListAsync(cancellationToken);
        }
        return new List<User>();
    }

    public Task<UserToken?> FindTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken = default)
        => Context.UserTokens.FindAsync(new object[] { user.Id, loginProvider, name }, cancellationToken).AsTask();

    public Task AddUserTokenAsync(UserToken token)
    {
        Context.UserTokens.Add(token);
        // TODO: await SaveChanges();
        // luu ý  Task.CompletedTask;
        return Task.CompletedTask;
    }

    public Task RemoveUserTokenAsync(UserToken token)
    {
        Context.UserTokens.Remove(token);
        // TODO: await SaveChanges();
        // luu ý  Task.CompletedTask;
        return Task.CompletedTask;
    }

    public async Task RemoveTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var entry = await FindTokenAsync(user, loginProvider, name, cancellationToken).ConfigureAwait(false);
        if (entry != null)
        {
            await RemoveUserTokenAsync(entry).ConfigureAwait(false);
        }
    }

    public async Task<string?> GetTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var entry = await FindTokenAsync(user, loginProvider, name, cancellationToken).ConfigureAwait(false);
        return entry?.Value;
    }

    public async Task SetTokenAsync(User user, string loginProvider, string name, string? value, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var token = await FindTokenAsync(user, loginProvider, name, cancellationToken).ConfigureAwait(false);
        if (token == null)
        {
            var userToken = new UserToken
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            };
            await AddUserTokenAsync(userToken).ConfigureAwait(false);
        }
        else
        {
            token.Value = value;
        }
    }

    public async Task<int> CountCodesAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken).ConfigureAwait(false) ?? "";
        if (mergedCodes.Length > 0)
        {
            return mergedCodes.Split(';').Length;
        }
        return 0;
    }

    public Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken = default)
    {
        var mergedCodes = string.Join(";", recoveryCodes);
        return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
    }

    public async Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        if (code == null)
        {
            throw new ArgumentNullException(nameof(code));
        }

        var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken).ConfigureAwait(false) ?? "";
        var splitCodes = mergedCodes.Split(';');
        if (splitCodes.Contains(code))
        {
            var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
            await ReplaceCodesAsync(user, updatedCodes, cancellationToken).ConfigureAwait(false);
            return true;
        }
        return false;
    }

    public Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken = default)
         => SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);

    public Task<string?> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken = default)
         => GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);

}
