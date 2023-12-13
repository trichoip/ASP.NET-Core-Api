using System.Text.Json.Serialization;

namespace AspNetCore.Client.OData.Models;

public class Backpack
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int EmployeeId { get; set; }
    [JsonIgnore]
    public Employee Employee { get; set; }
}
