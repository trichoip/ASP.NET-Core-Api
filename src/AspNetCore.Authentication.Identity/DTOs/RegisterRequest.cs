using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Authentication.Identity.DTOs;

public sealed class RegisterRequest
{
    [EmailAddress]
    public required string Email { get; init; }

    [DefaultValue("aA123456!")]
    public required string Password { get; init; }

    //[DefaultValue("aA123456!")]
    //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //public string ConfirmPassword { get; set; }
}
