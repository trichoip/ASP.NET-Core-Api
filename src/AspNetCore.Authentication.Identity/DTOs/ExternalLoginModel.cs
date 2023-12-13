using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Authentication.Identity.DTOs;

public class ExternalLoginModel
{
    [EmailAddress]
    public string Email { get; set; }
}
