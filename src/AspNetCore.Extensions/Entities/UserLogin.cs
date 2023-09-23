namespace AspNetCore.Extensions.Entities
{
    public class UserLogin
    {
        public virtual string LoginProvider { get; set; } = default!;

        public virtual string ProviderKey { get; set; } = default!;

        public virtual string? ProviderDisplayName { get; set; }

        public virtual Guid UserId { get; set; } = default!;
    }
}
