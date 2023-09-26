using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SyntaxController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTodoItems()
    {
        int num = 1;

        var a = num switch
        {
            1 => "asas",
            _ => " as",
        };

        (string age, string name) customer = new()
        {
            age = "1",
            name = "2"
        };

        var age1 = customer.age;
        var name1 = customer.name;

        (string age, string name) customer2 = new("3", "4");

        var customer3 = new { age3 = "5", nam4e = "6" };

        var customer4 = (age1, name1);

        var customer5 = (hh: age1, gg: name1);

        var customer6 = new { age1, name1 };

        return Ok(new { a, age1, name1, customer, customer2, customer3, customer4, customer5, customer6 });
    }
}
