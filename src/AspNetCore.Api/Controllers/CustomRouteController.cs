using AspNetCore.Api.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Controllers
{
    [MyApiController]
    public class CustomRouteController : ControllerBase
    {

        [HttpGet]
        [MyApiController]
        public IActionResult Custom()
        {
            return Ok();
        }
    }
}
