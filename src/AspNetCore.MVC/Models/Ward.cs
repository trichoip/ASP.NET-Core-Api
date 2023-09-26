using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class Ward
    {
        public Ward()
        {
            Addresses = new HashSet<Address>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? DistrictId { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
