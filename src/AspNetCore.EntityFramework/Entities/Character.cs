using AspNetCore.EntityFramework.Entities.Common;

namespace AspNetCore.EntityFramework.Entities
{
    public class Character : BaseAuditableEntity
    {
        public int Id { get; set; }

        //[Column(TypeName = "nvarchar(30)")]
        public string? Name { get; set; }

        //[Column(TypeName = "nvarchar(24)")] // convert enum sang string khi lưu lên db, đây là cách 1 , cách 2 3 xem trong dbcontext
        //[EnumDataType(typeof(OrderStatus), ErrorMessage = "enum not found")] // enum validation, nếu có thì nhập sai sẽ báo lỗi, không có nhập cái gì cũng nhận được
        //public OrderStatus Status { get; set; }

        // one to one
        public virtual Backpack Backpack { get; set; }

        // one to many
        public virtual List<Weapon> Weapons { get; set; }

        // many to many
        public virtual List<Faction> Factions { get; set; }
    }
}
