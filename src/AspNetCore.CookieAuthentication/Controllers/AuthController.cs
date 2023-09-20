using AspNetCore.CookieAuthentication.Models;
using AspNetCore.CookieAuthentication.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspNetCore.CookieAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        static IList<User> users = new List<User>()
        {
            new User(){Id = 1, PasswordHash = "1", Username ="1", Role = "Admin"},
            new User(){Id = 2, PasswordHash = "2", Username ="2", Role = "User"}
        };

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel objLoginModel)
        {
            var user = users.Where(x => x.Username == objLoginModel.UserName && x.PasswordHash == objLoginModel.Password).FirstOrDefault();
            if (user != null)
            {
                var claims = new List<Claim>() {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Role),
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    IsPersistent = false

                });

                return Ok("You are logged in");
            }

            return Unauthorized();
        }

        [HttpGet("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("You are logged out");
        }

        [HttpGet("GetClaim")]
        public async Task<IActionResult> GetClaim()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(User.Claims.ToDictionary(item => item.Type, item => item.Value));
            }
            return Ok("not Authenticated");
        }

        [HttpGet("User")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> UserRole()
        {
            return Ok(User.Identity.Name);
        }

        [HttpGet("Admin")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> AdminRole()
        {
            return Ok(User.Identity.Name);
        }

        [HttpGet("AdminUser")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = "Admin,User")]
        public async Task<IActionResult> AdminUserRole()
        {
            return Ok(User.Identity.Name);
        }

    }
}