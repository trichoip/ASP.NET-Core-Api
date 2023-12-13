using System.Collections.Generic;

#nullable disable

namespace AspNetCore.Client.MVC.Models;

public partial class Role
{
    public Role()
    {
        AccountRoles = new HashSet<AccountRole>();
    }

    public long Id { get; set; }
    public string Name { get; set; }

    public virtual ICollection<AccountRole> AccountRoles { get; set; }
}
