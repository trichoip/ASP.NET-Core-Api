namespace AspNetCore.Authentication.Identity.DTOs;

public sealed class TwoFactorResponse
{
    public required string SharedKey { get; init; }
    public required int RecoveryCodesLeft { get; init; }
    public string[]? RecoveryCodes { get; init; }
    public required bool IsTwoFactorEnabled { get; init; }
    public required bool IsMachineRemembered { get; init; }
    public string? AuthenticatorUri { get; init; }
}
