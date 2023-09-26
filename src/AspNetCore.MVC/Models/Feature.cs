using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class Feature
    {
        public Feature()
        {
            CarFeatures = new HashSet<CarFeature>();
        }

        public long Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CarFeature> CarFeatures { get; set; }
    }
}
