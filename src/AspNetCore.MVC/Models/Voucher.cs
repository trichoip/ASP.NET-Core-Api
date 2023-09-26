using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
            Books = new HashSet<Book>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Discription { get; set; }
        public DateTime? EndDate { get; set; }
        public string Image { get; set; }
        public int? MaxDiscount { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Percentage { get; set; }
        public DateTime? StartDate { get; set; }
        public string Status { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
