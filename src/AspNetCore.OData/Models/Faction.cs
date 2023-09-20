using System.Text.Json.Serialization;

namespace AspNetCore.OData.Models
{
    public class Faction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Employee> Employees { get; set; }
    }
}
