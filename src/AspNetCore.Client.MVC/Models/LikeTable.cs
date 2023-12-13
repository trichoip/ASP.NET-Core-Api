#nullable disable

namespace AspNetCore.Client.MVC.Models;

public partial class LikeTable
{
    public long AccountId { get; set; }
    public long CarId { get; set; }

    public virtual Account Account { get; set; }
    public virtual Car Car { get; set; }
}
