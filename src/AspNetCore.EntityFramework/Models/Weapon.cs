using System.Text.Json.Serialization;

namespace AspNetCore.EntityFramework.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // lưu ý: child phải có properties đúng với cấu trúc ClassFatherId hoặc PropertiesFatherId
        public int CharacterId { get; set; }
        [JsonIgnore]
        public virtual Character Nemo { get; set; }
    }
}
