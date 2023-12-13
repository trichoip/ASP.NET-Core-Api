using System.Text.Json.Serialization;

namespace AspNetCore.ObjectMapping.AutoMapper.Models;

public class Product
{
    public int ProductID { get; set; }
    public string? Name { get; set; }
    [JsonIgnore]
    public string? OptionalName { get; set; }
    [JsonIgnore]
    public string? Moon { get; set; }
    public string? Igone { get; set; }
    public int Quantity { get; set; }
    public int Amount { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
}
