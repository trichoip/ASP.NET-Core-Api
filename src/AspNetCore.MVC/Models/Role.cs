using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class Role
    {
        public Role()
        {
            AccountRoles = new HashSet<AccountRole>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AccountRole> AccountRoles { get; set; }
    }
}
