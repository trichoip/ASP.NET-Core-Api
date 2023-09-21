using System.Text.Json.Serialization;

namespace AspNetCore.EntityFramework.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // lưu ý: child phải có properties đúng với cấu trúc ClassFatherId hoặc PropertiesFatherId
        // int CharacterId -> khi migration thì nó tự thêm OnDelete(DeleteBehavior.Cascade)
        // int? CharacterId ->  khi migration thì nó không có thêm OnDelete(DeleteBehavior.Cascade)
        // nếu lúc đầu là int? thì khi migration thì nó không có thêm OnDelete(DeleteBehavior.Cascade)
        // nếu lúc sau sửa lại là int bỏ ? thì khi migration thì nó update OnDelete(DeleteBehavior.Cascade)
        // nêu lúc đầu là int thì nó thêm OnDelete(DeleteBehavior.Cascade) và lúc thêm ? thì nó xóa OnDelete(DeleteBehavior.Cascade)
        public int CharacterId { get; set; }
        [JsonIgnore]
        public virtual Character Nemo { get; set; }
    }
}
