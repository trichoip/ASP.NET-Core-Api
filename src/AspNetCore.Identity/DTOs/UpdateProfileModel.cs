using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.DTOs
{
    public class UpdateProfileModel
    {
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
