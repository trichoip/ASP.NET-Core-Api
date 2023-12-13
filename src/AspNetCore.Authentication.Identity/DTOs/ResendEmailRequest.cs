using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Authentication.Identity.DTOs;

public sealed class ResendEmailRequest
{
    [EmailAddress]
    public required string Email { get; init; }
}
