using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Authentication.Identity.DTOs;

public sealed class ForgotPasswordRequest
{
    [EmailAddress]
    public required string Email { get; init; }
}
