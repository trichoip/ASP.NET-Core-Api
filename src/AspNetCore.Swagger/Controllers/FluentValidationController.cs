using AspNetCore.Swagger.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Swagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FluentValidationController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create(Sample sample)
        {
            return Ok(sample);
        }
    }
}
