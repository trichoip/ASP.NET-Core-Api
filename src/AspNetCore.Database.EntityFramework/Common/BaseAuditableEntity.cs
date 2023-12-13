using AspNetCore.Database.EntityFramework.Common.Interfaces;

namespace AspNetCore.Database.EntityFramework.Common;

public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}
