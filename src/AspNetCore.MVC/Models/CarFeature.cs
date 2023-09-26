using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class CarFeature
    {
        public long CarId { get; set; }
        public long FeatureId { get; set; }

        public virtual Car Car { get; set; }
        public virtual Feature Feature { get; set; }
    }
}
