using System.Security.Claims;

namespace AspNetCore.Api.Extensions.Entities;

public class RoleClaim
{
    public virtual int Id { get; set; } = default!;

    public virtual Guid RoleId { get; set; } = default!;

    public virtual string? ClaimType { get; set; }

    public virtual string? ClaimValue { get; set; }

    public virtual Claim ToClaim()
    {
        return new Claim(ClaimType!, ClaimValue!);
    }

    public virtual void InitializeFromClaim(Claim? other)
    {
        ClaimType = other?.Type;
        ClaimValue = other?.Value;
    }
}
