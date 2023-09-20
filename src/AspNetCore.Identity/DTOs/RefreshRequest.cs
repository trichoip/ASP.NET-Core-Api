namespace AspNetCore.Identity.DTOs
{
    public sealed class RefreshRequest
    {
        public required string RefreshToken { get; init; }
    }
}
