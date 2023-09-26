using System.ComponentModel.DataAnnotations;

namespace asp.net_core_empty_5._0.ViewModel
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
