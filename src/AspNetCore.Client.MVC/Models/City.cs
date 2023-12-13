using System.Collections.Generic;

#nullable disable

namespace AspNetCore.Client.MVC.Models;

public partial class City
{
    public City()
    {
        Addresses = new HashSet<Address>();
        Districts = new HashSet<District>();
    }

    public long Id { get; set; }
    public string Code { get; set; }
    public string Image { get; set; }
    public string Name { get; set; }

    public virtual ICollection<Address> Addresses { get; set; }
    public virtual ICollection<District> Districts { get; set; }
}
