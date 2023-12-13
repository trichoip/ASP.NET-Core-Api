using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Authentication.Identity.DTOs;

public class UpdateProfileModel
{
    [Phone]
    public string PhoneNumber { get; set; }
}
