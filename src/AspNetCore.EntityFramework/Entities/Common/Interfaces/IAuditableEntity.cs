namespace AspNetCore.EntityFramework.Entities.Common.Interfaces
{
    public interface IAuditableEntity : IEntity
    {
        string? CreatedBy { get; set; }
        DateTimeOffset? CreatedAt { get; set; }
        string? ModifiedBy { get; set; }
        DateTimeOffset? ModifiedAt { get; set; }
    }
}
