using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.DTOs
{
    public class LoginWith2faModel
    {
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string TwoFactorCode { get; set; }

        public bool RememberMachine { get; set; }
    }
}
