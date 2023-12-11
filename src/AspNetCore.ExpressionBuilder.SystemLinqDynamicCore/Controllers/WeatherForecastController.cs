using AspNetCore.ExpressionBuilder.SystemLinqDynamicCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace AspNetCore.ExpressionBuilder.SystemLinqDynamicCore.Controllers;

[ApiController]
[Route("[controller]/[action]")]

// https://dynamic-linq.net/overview
public class WeatherForecastController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WeatherForecastController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Query()
    {
        string sumary = "Hot";

        var query2 = _context.WeatherForecasts.Where($"Summary == @0", sumary).ToQueryString();
        var query3 = _context.WeatherForecasts.Where($"Summary == @0 and (TemperatureC > @1 or TemperatureF < @2)", sumary, 2, 10).ToQueryString();
        var query1 = _context.WeatherForecasts.Where($"Summary == @0 && (TemperatureC > @1 || TemperatureF < @2)", sumary, 2, 10).ToQueryString();

        return Ok($"{query2} \n\n {query3} \n\n {query1}");
    }

    [HttpGet]
    public IActionResult Selecting()
    {

        var query1 = _context.WeatherForecasts.Select("new {Id, Summary}");

        return Ok(new { query = query1.ToQueryString(), data = query1.ToDynamicList() });
    }

    [HttpGet]
    public IActionResult Ordering()
    {
        var query1 = _context.WeatherForecasts.OrderBy("TemperatureC, Id, Summary").ToQueryString();
        var query2 = _context.WeatherForecasts.OrderBy("TemperatureC DESC, Summary desc").ToQueryString();

        return Ok($"{query1} \n\n {query2}");

    }

}
