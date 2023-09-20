using System.Text.Json.Serialization;

namespace AspNetCore.Identity.Models
{
    public class Faction
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
