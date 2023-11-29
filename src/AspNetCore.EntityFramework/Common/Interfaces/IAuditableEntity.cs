namespace AspNetCore.EntityFramework.Common.Interfaces
{
    public interface IAuditableEntity : IEntity
    {
        public string? CreatedBy { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
