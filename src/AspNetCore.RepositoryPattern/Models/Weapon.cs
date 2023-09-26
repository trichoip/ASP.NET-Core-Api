using System.Text.Json.Serialization;

namespace AspNetCore.RepositoryPattern.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CharacterId { get; set; }
        [JsonIgnore]
        public virtual Character Character { get; set; }
    }
}
