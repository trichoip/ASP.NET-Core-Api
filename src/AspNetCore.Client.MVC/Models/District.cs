using System.Collections.Generic;

#nullable disable

namespace AspNetCore.Client.MVC.Models;

public partial class District
{
    public District()
    {
        Addresses = new HashSet<Address>();
        Wards = new HashSet<Ward>();
    }

    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public long CityId { get; set; }

    public virtual City City { get; set; }
    public virtual ICollection<Address> Addresses { get; set; }
    public virtual ICollection<Ward> Wards { get; set; }
}
