using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.DTOs
{
    public sealed class ResendEmailRequest
    {
        [EmailAddress]
        public required string Email { get; init; }
    }
}
