using AspNetCore.Api.Record;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class RecordTypeController : ControllerBase
{

    [HttpGet]
    public IActionResult RecordDemo()
    {
        var member = new Member
        {
            Id = 1,
            FirstName = "Kirtesh",
            LastName = "Shah",
            Address = "Vadodara"
        };

        var newmember = member with
        {
            Address = "new Address",
            LastName = "new  LastName"
        };

        var last = member with
        {
            Address = "Vadodara"
        };

        return Ok(new { member, newmember, compare = last == member });
    }
}
