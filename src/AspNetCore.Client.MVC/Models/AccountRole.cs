#nullable disable

namespace AspNetCore.Client.MVC.Models;

public partial class AccountRole
{
    public long AccountId { get; set; }
    public long RoleId { get; set; }

    public virtual Account Account { get; set; }
    public virtual Role Role { get; set; }
}
