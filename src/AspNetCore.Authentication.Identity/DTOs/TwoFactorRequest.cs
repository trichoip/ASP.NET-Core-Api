using System.ComponentModel;

namespace AspNetCore.Authentication.Identity.DTOs;

public sealed class TwoFactorRequest
{
    [DefaultValue(false)]
    public bool? Enable { get; init; }

    public string? TwoFactorCode { get; init; }

    [DefaultValue(false)]
    public bool ResetSharedKey { get; init; }

    [DefaultValue(false)]
    public bool ResetRecoveryCodes { get; init; }

    [DefaultValue(false)]
    public bool ForgetMachine { get; init; }
}
