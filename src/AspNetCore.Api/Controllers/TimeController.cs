using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TimeController : ControllerBase
{

    [HttpGet]
    public IActionResult Time()
    {
        return Ok(new
        {
            TimeSpan_New = new TimeSpan(0, 0, 30),
            TimeSpan_FromDays = TimeSpan.FromDays(2),
            TimeSpan_FromHours = TimeSpan.FromHours(2),
            TimeSpan_FromMilliseconds = TimeSpan.FromMilliseconds(120000),
            TimeSpan_FromMinutes = TimeSpan.FromMinutes(5),
            TimeSpan_FromSeconds = TimeSpan.FromSeconds(120),
            TimeSpan_FromTicks = TimeSpan.FromTicks(60),
            TimeSpan_Parse = TimeSpan.Parse("00:00:30"),
            //TimeSpan_ParseExact = TimeSpan.ParseExact("00:00:30", "hh:mm:ss", null),
            TimeSpan_TryParse = TimeSpan.TryParse("00:00:30", out TimeSpan timeSpan),
            TimeSpan_TryParseExact = TimeSpan.TryParseExact("00:00:30", "hh:mm:ss", null, out TimeSpan timeSpan1),
            DateTime_Now = DateTime.Now,
            DateTime_UtcNow = DateTime.UtcNow,
            DateTime_Today = DateTime.Today,
            DateTime_Parse = DateTime.Parse("2020-01-01"),
            DateTime_ParseExact = DateTime.ParseExact("2020-01-01", "yyyy-MM-dd", null),
            DateTime_TryParse = DateTime.TryParse("2020-01-01", out DateTime dateTime),
            DateTime_Now_AddMinutes = DateTime.Now.AddMinutes(30),
            DateTime_Now_AddHours = DateTime.Now.AddHours(30),
            DateTime_Now_AddDays = DateTime.Now.AddDays(30),
            DateTime_Now_AddMonths = DateTime.Now.AddMonths(30),
            DateTime_Now_AddYears = DateTime.Now.AddYears(30),
            DateTime_Now_AddSeconds = DateTime.Now.AddSeconds(30),
            DateTime_Now_AddTicks = DateTime.Now.AddTicks(30),
            DateTime_Now_Add = DateTime.Now.Add(new TimeSpan(0, 0, 30)),
            DateTime_Now_AddMilliseconds = DateTime.Now.AddMilliseconds(30),
            DateTime_Now_AddHours_AddMinutes_AddSeconds = DateTime.Now.AddHours(3).AddMinutes(30).AddSeconds(120),
        });
    }
}
