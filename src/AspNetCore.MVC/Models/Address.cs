using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class Address
    {
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Street { get; set; }
        public long CityId { get; set; }
        public long DistrictId { get; set; }
        public long WardId { get; set; }

        public virtual City City { get; set; }
        public virtual District District { get; set; }
        public virtual Ward Ward { get; set; }
        public virtual Car Car { get; set; }
    }
}
