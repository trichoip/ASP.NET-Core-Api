using AspNetCore.EntityFramework.Entities.Common;

namespace AspNetCore.EntityFramework.Entities
{
    public class Character : BaseAuditableEntity
    {
        public int Id { get; set; }

        //[Column(TypeName = "nvarchar(30)")]
        public string? Name { get; set; }

        // one to one
        public virtual Backpack Backpack { get; set; }

        // one to many
        public virtual List<Weapon> Weapons { get; set; }

        // many to many
        public virtual List<Faction> Factions { get; set; }
    }
}
