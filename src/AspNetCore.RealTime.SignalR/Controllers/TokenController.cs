using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AspNetCore.RealTime.SignalR.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> GetTokenForCredentialsAsync(LoginRequest login)
    {
        var result = login.Username == "admin" && login.Password == "admin" ? true : false;
        if (!result)
        {
            result = login.Username == "user" && login.Password == "user" ? true : false;
        }

        return result ? Ok(GenerateToken(login.Username)) : Unauthorized();
    }

    private string GenerateToken(string userId)
    {
        var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes("demokeydemokeydemokeydemokeydemokeydemokey"));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, userId)
        };

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            "signalrdemo",
            "signalrdemo",
            claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}