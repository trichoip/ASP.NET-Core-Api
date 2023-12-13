using AspNetCore.Api.FluentValidations.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.FluentValidations.Controllers;

[ApiController]
[Route("[controller]")]
public class ValidatorController : ControllerBase
{

    [HttpPost]
    public IActionResult Create(Customer customer)
    {
        return Ok(customer);
    }

}