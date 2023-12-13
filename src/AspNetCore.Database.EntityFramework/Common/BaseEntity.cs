using AspNetCore.Database.EntityFramework.Common.Interfaces;

namespace AspNetCore.Database.EntityFramework.Common;

public abstract class BaseEntity : IEntity
{
    public int Id { get; set; }

}
