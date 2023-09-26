using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

#nullable disable

namespace asp.net_core_empty_5._0.Models
{
    public partial class Account
    {
        public Account()
        {
            AccountRoles = new HashSet<AccountRole>();
            Books = new HashSet<Book>();
            Cars = new HashSet<Car>();
            Notifications = new HashSet<Notification>();
        }

        [Key]// primary key nếu pk không phải classnameID hoặc ID thì phải dùng [Key] để xác định pk
        public long Id { get; set; }
        [Display(Name = "Avatar123")]
        public string Avatar { get; set; }
        public double? Balance { get; set; }
        public DateTime? BirthDate { get; set; }
        public string CreatedBy { get; set; }

        [Remote(action: "VerifyEmail", controller: "Validation")]
        [EmailAddress(ErrorMessage = "Name email")]
        public string Email { get; set; }
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? JoinDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [Required(ErrorMessage = "required is")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "5 - 10")]
        public string Name { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
        public string Thumnail { get; set; }
        public string Username { get; set; }

        public virtual DrivingLicense DrivingLicense { get; set; }
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
