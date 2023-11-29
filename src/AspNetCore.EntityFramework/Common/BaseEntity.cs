using AspNetCore.EntityFramework.Common.Interfaces;

namespace AspNetCore.EntityFramework.Common
{
    public abstract class BaseEntity : IEntity
    {
        public int Id { get; set; }

    }
}
