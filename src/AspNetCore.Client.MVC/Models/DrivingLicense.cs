using System;

#nullable disable

namespace AspNetCore.Client.MVC.Models;

public partial class DrivingLicense
{
    public long Id { get; set; }
    public DateTime? BirthDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string ImageFront { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string Name { get; set; }
    public long? NumberDrivingLicense { get; set; }
    public string Status { get; set; }

    public virtual Account IdNavigation { get; set; }
}
