using System;
using System.Collections.Generic;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class Book
    {
        public long Id { get; set; }
        public DateTime? BookDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Price { get; set; }
        public DateTime? StartDate { get; set; }
        public string Status { get; set; }
        public double? TotalPrice { get; set; }
        public long AccountId { get; set; }
        public long CarId { get; set; }
        public long? VoucherId { get; set; }

        public virtual Account Account { get; set; }
        public virtual Car Car { get; set; }
        public virtual Voucher Voucher { get; set; }
        public virtual Review Review { get; set; }
    }
}
