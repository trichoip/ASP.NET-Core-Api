using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Extensions.Entities
{
    public class UserToken
    {
        public virtual Guid UserId { get; set; } = default!;

        public virtual string LoginProvider { get; set; } = default!;

        public virtual string Name { get; set; } = default!;

        [ProtectedPersonalData]
        public virtual string? Value { get; set; }
    }
}
