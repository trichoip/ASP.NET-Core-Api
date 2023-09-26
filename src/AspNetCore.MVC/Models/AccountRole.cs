using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class AccountRole
    {
        public long AccountId { get; set; }
        public long RoleId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }
    }
}
