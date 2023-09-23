using System.Security.Claims;

namespace AspNetCore.Extensions.Entities
{
    public class UserClaim
    {
        public virtual int Id { get; set; } = default!;

        public virtual Guid UserId { get; set; } = default!;

        public virtual string? ClaimType { get; set; }

        public virtual string? ClaimValue { get; set; }

        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType!, ClaimValue!);
        }

        public virtual void InitializeFromClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }
    }
}
