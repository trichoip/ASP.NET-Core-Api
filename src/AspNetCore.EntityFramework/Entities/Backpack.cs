using AspNetCore.EntityFramework.Entities.Common;
using System.Text.Json.Serialization;

namespace AspNetCore.EntityFramework.Entities
{
    public class Backpack : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string Description { get; set; }

        // lưu ý: child phải có properties đúng với cấu trúc ClassFatherId hoặc PropertiesFatherId
        // ví dụ như Character Nemo thì CharacterId hoặc NemoId 
        public int NemoId { get; set; }
        [JsonIgnore]
        public virtual Character Nemo { get; set; }
    }
}
