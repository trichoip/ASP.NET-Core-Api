using AspNetCore.Authentication.JWT.Helpers;
using AspNetCore.Authentication.JWT.Models;
using AspNetCore.Authentication.JWT.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Authentication.JWT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    public static User user = new User();
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        PasswordHashHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        user.Username = request.Username;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        if (user.Username != request.Username)
        {
            return BadRequest("User not found.");
        }

        if (!PasswordHashHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Wrong password.");
        }

        string token = TokenHelper.CreateToken(user, _configuration);
        var refreshToken = TokenHelper.GenerateRefreshToken();

        SetRefreshToken(refreshToken);

        return Ok(new
        {
            token,
            refreshToken
        });
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (!user.RefreshToken.Equals(refreshToken))
        {
            return Unauthorized("Invalid Refresh Token.");
        }
        else if (user.TokenExpires < DateTime.Now)
        {
            return Unauthorized("Token expired.");
        }

        string token = TokenHelper.CreateToken(user, _configuration);
        var newRefreshToken = TokenHelper.GenerateRefreshToken();
        SetRefreshToken(newRefreshToken);

        return Ok(new
        {
            token,
            newRefreshToken
        });
    }

    private void SetRefreshToken(RefreshToken newRefreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;
    }

}