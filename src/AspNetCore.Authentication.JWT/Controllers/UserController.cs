using AspNetCore.Authentication.JWT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspNetCore.Authentication.JWT.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var username1 = User?.Identity?.Name;
            var username2 = User.FindFirstValue(ClaimTypes.Name);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var userName = _userService.GetMyName();
            return Ok(new { userName, username1, username2, role });
        }

        [HttpPost]
        //[PermissionAuthorize(new[] { "User" })]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> User1()
        {
            return Ok("Hello World");
        }

        [HttpPost]
        //[PermissionAuthorize(new[] { "Admin" })]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            return Ok("Hello World");
        }

        [HttpPost]
        //[PermissionAuthorize("Admin", "User")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AdminUser()
        {
            return Ok("Hello World");
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> Modifiel()
        {
            return Ok("Hello World");
        }
    }
}
