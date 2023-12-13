using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Authentication.Identity.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    [PersonalData]
    public string? CustomTag { get; set; }

    //public bool UserNameConfirmed { get; set; }

    public virtual Backpack? Backpack { get; set; }

    public virtual ICollection<Weapon> Weapons { get; set; }

    public virtual ICollection<Faction> Factions { get; set; }

}
