using AspNetCore.Api.Extensions.Data;
using AspNetCore.Api.Extensions.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AspNetCore.Api.Extensions.Repositories;

public class RoleStore : IRoleStore
{
    private readonly ApplicationDbContext Context;
    public bool AutoSaveChanges { get; set; } = true;

    public RoleStore(ApplicationDbContext context)
    {
        Context = context;
    }
    public async Task SaveChanges(CancellationToken cancellationToken)
    {
        if (AutoSaveChanges) await Context.SaveChangesAsync(cancellationToken);
    }

    public IQueryable<Role> Roles => Context.Set<Role>();

    public async Task<Role> CreateAsync(Role role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        Context.Add(role);
        await SaveChanges(cancellationToken);
        return role;
    }

    public async Task<Role> UpdateAsync(Role role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        Context.Attach(role);
        role.ConcurrencyStamp = Guid.NewGuid().ToString();
        Context.Update(role);
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
            throw new Exception("Error updating role.", exception);
        }
        return role;
    }

    public async Task DeleteAsync(Role role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        Context.Remove(role);
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
            throw new Exception("Error Delete role.", exception);
        }

    }
    public Task<Role?> FindByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Roles.FirstOrDefaultAsync(u => u.Id.Equals(roleId), cancellationToken);
    }

    public Task<Role?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
    }

    public Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
    {
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }
        var roleClaim = new RoleClaim { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
        Context.RoleClaims.Add(roleClaim);
        // TODO: SaveChanges
        return Task.FromResult(false);
    }

    public async Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default)
    {
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }

        return await Context.RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType!, c.ClaimValue!)).ToListAsync(cancellationToken);
    }

    public async Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
    {
        if (role == null)
        {
            throw new ArgumentNullException(nameof(role));
        }
        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }
        var claims = await Context.RoleClaims.Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type).ToListAsync(cancellationToken);
        foreach (var c in claims)
        {
            Context.RoleClaims.Remove(c);
            // TODO: SaveChanges
        }
    }

}
