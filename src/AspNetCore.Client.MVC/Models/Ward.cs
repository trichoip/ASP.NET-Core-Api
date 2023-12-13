using System.Collections.Generic;

#nullable disable

namespace AspNetCore.Client.MVC.Models;

public partial class Ward
{
    public Ward()
    {
        Addresses = new HashSet<Address>();
    }

    public long Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public long? DistrictId { get; set; }

    public virtual District District { get; set; }
    public virtual ICollection<Address> Addresses { get; set; }
}
