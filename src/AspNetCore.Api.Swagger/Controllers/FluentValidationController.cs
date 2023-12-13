using AspNetCore.Api.Swagger.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Swagger.Controllers;

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
