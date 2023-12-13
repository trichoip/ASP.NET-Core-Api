namespace AspNetCore.Api.Extensions.Entities;

public class Role
{
    public Role() { }

    public Role(string roleName) : this()
    {
        Name = roleName;
    }
    public virtual Guid Id { get; set; } = default!;

    public virtual string? Name { get; set; }

    public virtual string? NormalizedName { get; set; }

    public virtual string? ConcurrencyStamp { get; set; }

}
