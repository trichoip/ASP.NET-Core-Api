using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.DTOs
{
    public sealed class ForgotPasswordRequest
    {
        [EmailAddress]
        public required string Email { get; init; }
    }
}
