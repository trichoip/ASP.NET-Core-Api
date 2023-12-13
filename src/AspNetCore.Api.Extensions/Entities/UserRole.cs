namespace AspNetCore.Api.Extensions.Entities;

public class UserRole
{
    public virtual Guid UserId { get; set; } = default!;

    public virtual Guid RoleId { get; set; } = default!;
}
