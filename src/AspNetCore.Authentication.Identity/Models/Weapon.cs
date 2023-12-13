using System.Text.Json.Serialization;

namespace AspNetCore.Authentication.Identity.Models;

public class Weapon
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Guid UserId { get; set; }

    [JsonIgnore]
    public virtual ApplicationUser User { get; set; }
}
