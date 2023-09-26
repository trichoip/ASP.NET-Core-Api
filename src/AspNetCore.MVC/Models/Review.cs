using System;
using System.Collections.Generic;

#nullable disable

namespace AspNetCore.MVC.Models
{
    public partial class Review
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? ReviewDate { get; set; }
        public int? StarReview { get; set; }
        public string Status { get; set; }

        public virtual Book IdNavigation { get; set; }
    }
}
