using System;
using System.Collections.Generic;

#nullable disable

namespace AspNetCore.MVC.Models
{
    public partial class Notification
    {
        public long Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Discription { get; set; }
        public bool? IsRead { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ShortDiscription { get; set; }
        public string Title { get; set; }
        public long? AccountId { get; set; }

        public virtual Account Account { get; set; }
    }
}
