using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AspNetCore.Client.HttpClient.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    [HttpPost("token")]
    public IActionResult Token([FromServices] IConfiguration _configuration)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Name"),
            new Claim(ClaimTypes.Role, "Admin"),
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SerectKey").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { jwt });
    }
}