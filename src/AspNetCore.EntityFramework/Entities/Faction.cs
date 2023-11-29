using AspNetCore.EntityFramework.Common;
using System.Text.Json.Serialization;

namespace AspNetCore.EntityFramework.Entities
{
    public class Faction : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public virtual List<Character> Characters { get; set; }
    }
}
