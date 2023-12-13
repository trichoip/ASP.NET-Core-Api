using AspNetCore.Authentication.Identity.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Octokit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AspNetCore.Authentication.Identity.Helpers;

public static class TokenHelper<T> where T : IdentityUser<Guid>, new()
{
    public static async Task<AccessTokenResponse> CreateToken(
        SignInManager<T> signInManager,
        IConfiguration configuration,
        IDataProtectionProvider dataProtectionProvider,
        string? username = null,
        T? user = null)
    {
        if (signInManager is null) throw new ArgumentNullException(nameof(signInManager));
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (dataProtectionProvider is null) throw new ArgumentNullException(nameof(dataProtectionProvider));
        if (string.IsNullOrEmpty(username) && user is null) throw new BadHttpRequestException("Username or User must be provided");

        if (user == null)
        {
            user = await signInManager.UserManager.FindByNameAsync(username!) is { } a ? a : throw new BadHttpRequestException("User not found");
            //user = await signInManager.UserManager.FindByNameAsync(username!) ?? throw new BadHttpRequestException("User not found");
            //user = await signInManager.UserManager.FindByNameAsync(username!) is T b ? b : throw new BadHttpRequestException("User not found");
        }

        #region Cach 1 - phèn
        //var role = await signInManager.GetRolesAsync(user!);
        //List<Claim> claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.NameIdentifier, user!.Id.ToString()), // required
        //    //new Claim(JwtRegisteredClaimNames.Sub, user!.Id.ToString()), // này là NameIdentifier khi đưa về nó tự map qua NameIdentifier
        //    new Claim(ClaimTypes.Name, user.UserName!),
        //    new Claim(ClaimTypes.Email, user.Email!),
        //    new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //};
        //role.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r))); 
        #endregion

        // cách 2 - Good
        var claimsPrincipal = await signInManager.CreateUserPrincipalAsync(user);
        IEnumerable<Claim> claims = claimsPrincipal.Claims;

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("Authentication:Schemes:Bearer:SerectKey").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        var protector = dataProtectionProvider.CreateProtector("RefreshToken");
        var ticketDataFormat = new TicketDataFormat(protector);

        var utcNow = DateTimeOffset.UtcNow;

        var response = new AccessTokenResponse
        {
            AccessToken = jwt,
            ExpiresIn = (long)TimeSpan.FromHours(1).TotalSeconds,
            RefreshToken = ticketDataFormat.Protect(CreateRefreshTicket(claimsPrincipal, utcNow)),
        };

        return response;
    }

    public static async Task<T> CheckValidRefreshToken(
        string refreshToken,
        SignInManager<T> signInManager,
        IDataProtectionProvider dataProtectionProvider)
    {
        if (signInManager is null) throw new ArgumentNullException(nameof(signInManager));
        if (dataProtectionProvider is null) throw new ArgumentNullException(nameof(dataProtectionProvider));
        if (string.IsNullOrEmpty(refreshToken)) throw new BadHttpRequestException("RefreshToken must be provided");

        var protector = dataProtectionProvider.CreateProtector("RefreshToken");
        var ticketDataFormat = new TicketDataFormat(protector);
        var ticket = ticketDataFormat.Unprotect(refreshToken);

        if (ticket?.Properties?.ExpiresUtc is not { } expiresUtc ||
            DateTimeOffset.UtcNow >= expiresUtc ||
            await signInManager.ValidateSecurityStampAsync(ticket.Principal) is not T user)
        {
            throw new AuthorizationException();
        }

        return user;
    }

    private static AuthenticationTicket CreateRefreshTicket(ClaimsPrincipal user, DateTimeOffset utcNow)
    {
        var refreshProperties = new AuthenticationProperties
        {
            ExpiresUtc = utcNow.AddDays(14)
        };

        return new AuthenticationTicket(user, refreshProperties, $"Bearer:RefreshToken");
    }
}
