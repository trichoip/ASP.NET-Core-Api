using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Authentication.Identity.DTOs;

public sealed class LoginRequest
{
    [EmailAddress]
    [DefaultValue("developermode549@gmail.com")]
    public required string Email { get; init; }

    [DefaultValue("aA123456!")]
    public required string Password { get; init; }

    public string? TwoFactorCode { get; init; }
    public string? TwoFactorRecoveryCode { get; init; }
}
