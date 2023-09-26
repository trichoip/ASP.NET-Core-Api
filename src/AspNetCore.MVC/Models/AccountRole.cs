using System;
using System.Collections.Generic;

#nullable disable

namespace AspNetCore.MVC.Models
{
    public partial class AccountRole
    {
        public long AccountId { get; set; }
        public long RoleId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }
    }
}
