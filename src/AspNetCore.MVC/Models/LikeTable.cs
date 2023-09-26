using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class LikeTable
    {
        public long AccountId { get; set; }
        public long CarId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Car Car { get; set; }
    }
}
