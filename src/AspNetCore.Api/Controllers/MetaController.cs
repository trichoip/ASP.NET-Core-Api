using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetCore.Api.Controllers;

public class MetaController : ControllerBase
{
    [HttpGet("/info")]
    public ActionResult<string> Info()
    {
        var assembly = typeof(Program).Assembly;

        var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
        var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

        return Ok($"Version: {version}, Last Updated: {lastUpdate}");
    }

    [HttpGet("/ip")]
    public IActionResult GenerateIPAddress()
    {

        if (Request.Headers.ContainsKey("X-Forwarded-For"))
            return Ok(Request.Headers["X-Forwarded-For"]);
        else
            return Ok(HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());

    }
}
