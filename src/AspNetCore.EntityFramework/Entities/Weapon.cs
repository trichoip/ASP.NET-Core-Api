using AspNetCore.EntityFramework.Entities.Common;
using System.Text.Json.Serialization;

namespace AspNetCore.EntityFramework.Entities
{
    public class Weapon : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // lưu ý: child phải có properties đúng với cấu trúc ClassFatherId hoặc PropertiesFatherId
        // int CharacterId -> khi migration thì nó tự thêm OnDelete(DeleteBehavior.Cascade) và CharacterId là not null
        // int? CharacterId ->  khi migration thì nó không có thêm OnDelete(DeleteBehavior.Cascade) và CharacterId là null
        // nếu lúc đầu là int? thì khi migration thì nó không có thêm OnDelete(DeleteBehavior.Cascade) và CharacterId là null
        // nếu lúc sau sửa lại là int bỏ ? thì khi migration thì nó update OnDelete(DeleteBehavior.Cascade) và CharacterId là not null
        // nêu lúc đầu là int thì nó thêm OnDelete(DeleteBehavior.Cascade) và lúc thêm ? thì nó xóa OnDelete(DeleteBehavior.Cascade) và CharacterId là null

        //nếu không cấu hình OnDelete(DeleteBehavior.Cascade) trong OnModelCreating thì:
        // int? CharacterId là null 
        // int CharacterId là not null
        // còn Character? Nemo hay Character Nemo thì không có gì thay đổi cả, null hay not null phụ thuộc vào int? CharacterId hay int CharacterId

        // còn nếu cấu hình OnDelete(DeleteBehavior.Cascade) trong OnModelCreating thì:
        // nếu cả 2 (int CharacterId) và (Character Nemo) hoặc 1 trong 2 mà không có '?' thì là not null
        // nó chỉ set null khi cả 2 int? CharacterId và Character? Nemo đều có '?' thì mới là null
        // - cách khác là cấu hình IsRequired: -> b.Property(_ => _.CharacterId).IsRequired(false);
        // - hoặc cấu hình IsRequired: -> trong b.HasMany(_ => _.Weapons).WithOne(_ => _.Nemo).IsRequired(false).OnDelete(DeleteBehavior.Cascade);
        // và lưu ý: (int? CharacterId) bắt buộc phải có '?' thì mới là set null, nếu không thì bị lỗi
        // và không cần cấu hình thêm ? cho (Character Nemo) thì nó vẫn là set null
        public int CharacterId { get; set; }
        [JsonIgnore]
        public virtual Character Nemo { get; set; }
    }
}
