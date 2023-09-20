using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.DTOs
{
    public class ExternalLoginModel
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
