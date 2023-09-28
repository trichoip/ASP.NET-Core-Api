using AspNetCore.EntityFramework.Entities.Common.Interfaces;

namespace AspNetCore.EntityFramework.Entities.Common
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }

    }
}
