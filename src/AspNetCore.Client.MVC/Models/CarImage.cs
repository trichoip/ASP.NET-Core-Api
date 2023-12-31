﻿using System;

#nullable disable

namespace AspNetCore.Client.MVC.Models;

public partial class CarImage
{
    public long Id { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string Image { get; set; }
    public string ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public long CarId { get; set; }

    public virtual Car Car { get; set; }
}
