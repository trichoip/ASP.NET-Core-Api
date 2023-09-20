using System.Text.Json.Serialization;

namespace AspNetCore.OData.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmployeeId { get; set; }
        [JsonIgnore]
        public Employee Employee { get; set; }
    }
}
