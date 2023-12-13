using AspNetCore.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace AspNetCore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HttpAttributeController : ControllerBase
{

    [HttpPost("Buy")]
    public IActionResult Edit()
    {
        return Ok();
    }

    [HttpPut]
    [Consumes(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml, "application/x-www-form-urlencoded")]
    [Produces(MediaTypeNames.Application.Json)]
    public ActionResult FromBody(TodoItem todoItem)
    {
        return Ok(todoItem);
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetId(int id)
    {
        return Ok(id);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok(id);
    }
}
