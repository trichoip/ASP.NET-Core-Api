using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Authentication.Identity.DTOs;

public sealed class ResetPasswordRequest
{
    [EmailAddress]
    public required string Email { get; init; }

    [DefaultValue("aA123456!")]
    public required string NewPassword { get; init; }

    //[DefaultValue("aA123456!")]
    //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    //public string ConfirmPassword { get; set; }

    public required string ResetCode { get; init; }
}
