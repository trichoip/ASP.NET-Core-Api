using System;
using System.Collections.Generic;

#nullable disable

namespace AspNetCore.MVC.Models
{
    public partial class CarFeature
    {
        public long CarId { get; set; }
        public long FeatureId { get; set; }

        public virtual Car Car { get; set; }
        public virtual Feature Feature { get; set; }
    }
}
