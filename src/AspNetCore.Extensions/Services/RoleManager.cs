using AspNetCore.Extensions.Entities;
using AspNetCore.Extensions.Repositories;
using System.Security.Claims;

namespace AspNetCore.Extensions.Services
{
    public class RoleManager : IRoleManager
    {
        protected virtual CancellationToken CancellationToken => CancellationToken.None;
        private readonly IRoleStore Store;

        public RoleManager(IRoleStore store)
        {
            Store = store;

        }

        public IQueryable<Role> Roles { get { return Store.Roles; } }

        public async Task<Role> CreateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            await ValidateRoleAsync(role).ConfigureAwait(false);
            role.NormalizedName = role.Name.Normalize().ToUpperInvariant();
            return await Store.CreateAsync(role, CancellationToken).ConfigureAwait(false);
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            await ValidateRoleAsync(role).ConfigureAwait(false);

            role.NormalizedName = role.Name.Normalize().ToUpperInvariant();
            return await Store.UpdateAsync(role, CancellationToken).ConfigureAwait(false);

        }

        public Task DeleteAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Store.DeleteAsync(role, CancellationToken);
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            if (roleName == null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            return await FindByNameAsync(roleName).ConfigureAwait(false) != null;
        }

        public Task<Role?> FindByIdAsync(Guid roleId)
        {
            return Store.FindByIdAsync(roleId, CancellationToken);
        }

        public async Task ValidateRoleAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (string.IsNullOrWhiteSpace(role.Name))
            {
                throw new ArgumentNullException(nameof(role.Name));
            }
            else
            {
                var owner = await FindByNameAsync(role.Name).ConfigureAwait(false);
                if (owner != null && !string.Equals(owner.Id, role.Id))
                {
                    throw new InvalidOperationException($"Role name '{role.Name}' is already taken.");
                }

            }
        }

        public Task<Role?> FindByNameAsync(string roleName)
        {
            if (roleName == null)
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            return Store.FindByNameAsync(roleName.Normalize().ToUpperInvariant(), CancellationToken);
        }

        public async Task<Role> AddClaimAsync(Role role, Claim claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            await Store.AddClaimAsync(role, claim, CancellationToken).ConfigureAwait(false);
            return await UpdateAsync(role).ConfigureAwait(false);
        }

        public async Task<Role> RemoveClaimAsync(Role role, Claim claim)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            await Store.RemoveClaimAsync(role, claim, CancellationToken).ConfigureAwait(false);
            return await UpdateAsync(role).ConfigureAwait(false);
        }

        public Task<IList<Claim>> GetClaimsAsync(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            return Store.GetClaimsAsync(role, CancellationToken);
        }

    }
}
