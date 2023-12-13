using AspNetCore.Authentication.Identity.Constants;
using AspNetCore.Authentication.Identity.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AspNetCore.Authentication.Identity.Helpers;

public class TokenHelper2<T> where T : IdentityUser<int>, new()
{
    private readonly SignInManager<T> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IDataProtectionProvider _dataProtectionProvider;
    private readonly IDataProtector protector;
    private readonly TicketDataFormat ticketDataFormat;

    public TokenHelper2(
        SignInManager<T> signInManager,
        IConfiguration configuration,
        IDataProtectionProvider dataProtectionProvider)
    {
        _signInManager = signInManager;
        _configuration = configuration;
        _dataProtectionProvider = dataProtectionProvider;

        protector = _dataProtectionProvider.CreateProtector(Token.RefreshToken);
        ticketDataFormat = new TicketDataFormat(protector);
    }

    public async Task<AccessTokenResponse> CreateTokenAsync(string? username = null, T? user = null)
    {
        if (string.IsNullOrEmpty(username) && user is null) throw new ArgumentException("Username or User must be provided");

        if (user == null)
        {
            user = await _signInManager.UserManager.FindByNameAsync(username!) ?? throw new ArgumentNullException("User not found", nameof(user));
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

        var key = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("Authentication:Schemes:Bearer:SerectKey").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            claims: claimsPrincipal.Claims,
            expires: DateTime.UtcNow.AddHours(Token.ExpiresIn),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        var response = new AccessTokenResponse
        {
            AccessToken = jwt,
            ExpiresIn = (long)TimeSpan.FromHours(Token.ExpiresIn).TotalSeconds,
            RefreshToken = ticketDataFormat.Protect(CreateRefreshTicket(claimsPrincipal, DateTimeOffset.UtcNow)),
        };
        return response;
    }

    public async Task<T> ValidateRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentNullException("RefreshToken must be provided", nameof(refreshToken));

        var ticket = ticketDataFormat.Unprotect(refreshToken);

        if (ticket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            DateTimeOffset.UtcNow >= expiresUtc ||
            await _signInManager.ValidateSecurityStampAsync(ticket.Principal) is not T user)
        {
            throw new UnauthorizedAccessException("Refresh token is not valid!");
        }
        return user;
    }

    private static AuthenticationTicket CreateRefreshTicket(ClaimsPrincipal user, DateTimeOffset utcNow)
    {
        var refreshProperties = new AuthenticationProperties
        {
            ExpiresUtc = utcNow.AddDays(14)
        };

        return new AuthenticationTicket(user, refreshProperties, Token.Bearer);
    }
}

