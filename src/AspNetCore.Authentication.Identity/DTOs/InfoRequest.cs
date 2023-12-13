using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Authentication.Identity.DTOs;

public sealed class InfoRequest
{
    [EmailAddress]
    public string? NewEmail { get; init; }
    public string? NewPassword { get; init; }
    public string? OldPassword { get; init; }
    [Phone]
    public string PhoneNumber { get; set; }
}
