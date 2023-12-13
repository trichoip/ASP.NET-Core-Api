﻿using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.TaskScheduler.Hangfire.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuartzController : ControllerBase
{
    [HttpGet]
    public IActionResult Quartz()
    {
        return Ok("running task in program.cs");
    }
}
