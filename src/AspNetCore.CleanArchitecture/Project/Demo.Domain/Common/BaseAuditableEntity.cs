using AspNetCore.CleanArchitecture.Project.Demo.Domain.Common.Interfaces;

namespace AspNetCore.CleanArchitecture.Project.Demo.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
}
