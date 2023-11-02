namespace AspNetCore.Identity.Constants;

public abstract class Token
{
    public const string RefreshToken = nameof(RefreshToken);
    public const string Bearer = nameof(Bearer);
    public const int ExpiresIn = 3;
}