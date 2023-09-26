using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class City
    {
        public City()
        {
            Addresses = new HashSet<Address>();
            Districts = new HashSet<District>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}
