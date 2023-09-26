using System.ComponentModel.DataAnnotations;

namespace AspNetCore.MVC.ViewModel
{
    public class AccountUserVM
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
