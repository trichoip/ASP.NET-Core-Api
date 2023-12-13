using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class IUrlHelperController : ControllerBase
{
    [HttpGet]
    public IActionResult Source()
    {
        var url = Url.Action("Destination");
        var url1 = Url.Action("Moon");
        var url2 = Url.Action("Destination", "IUrlHelper", new { id = 17, color = "red" });
        var url3 = Url.Action("Destination", "IUrlHelper", new { id = 17 }, protocol: Request.Scheme, host: Request.Host.Value, "fragment");
        var url4 = Url.ActionLink("Black", "IUrlHelper", new { id = 17 });
        var url5 = Url.RouteUrl("Black_Route", new { id = 17 });
        var url6 = Url.Link("Moon_Route", new { id = 17 });

        return Ok(new { url, url1, url2, url3, url4, url5, url6 });
    }

    [HttpGet("custom/url/to/destination", Name = "Destination_Route")]
    public IActionResult Destination()
    {
        return Ok();
    }

    [HttpGet(Name = "Moon_Route")]
    public IActionResult Moon()
    {
        return Ok();
    }

    [HttpGet("{id}", Name = "Black_Route")]
    public IActionResult Black(int id)
    {
        return Ok();
    }
}
