using System.Text.Json.Serialization;

namespace AspNetCore.RepositoryPattern.Models
{
    public class Backpack
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int CharacterId { get; set; }
        [JsonIgnore]
        public virtual Character Character { get; set; }
    }
}
