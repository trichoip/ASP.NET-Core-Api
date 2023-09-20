using System.Text.Json.Serialization;

namespace AspNetCore.Identity.Models
{
    public class Backpack
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual ApplicationUser User { get; set; }
    }
}
