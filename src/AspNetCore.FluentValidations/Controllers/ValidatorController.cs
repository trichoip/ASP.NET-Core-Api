using AspNetCore.FluentValidations.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.FluentValidations.Controllers
{
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
}