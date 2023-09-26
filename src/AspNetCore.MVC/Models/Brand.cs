using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Models = new HashSet<Model>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Model> Models { get; set; }
    }
}
