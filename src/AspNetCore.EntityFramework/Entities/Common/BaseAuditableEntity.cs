using AspNetCore.EntityFramework.Entities.Common.Interfaces;

namespace AspNetCore.EntityFramework.Entities.Common
{
    public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
    {
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
