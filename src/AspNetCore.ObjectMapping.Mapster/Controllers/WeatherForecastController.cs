using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.ObjectMapping.Mapster.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    WeatherForecast weatherForecast = new WeatherForecast
    {
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = 32,
        Summary = "Sumaary"

    };

    List<WeatherForecast> list;

    public WeatherForecastController()
    {
        list = new List<WeatherForecast> { weatherForecast, weatherForecast, weatherForecast, weatherForecast, weatherForecast, weatherForecast, };
    }

    [HttpGet]
    public IActionResult MappingNewObject()
    {
        var weatherForecastDTO = weatherForecast.Adapt<WeatherForecastDTO>();

        return Ok(new { weatherForecastDTO, change = weatherForecastDTO.Adapt<WeatherForecast>() });
    }

    [HttpGet]
    public IActionResult MappingExistingObject()
    {
        var repo = new WeatherForecastDTO();

        weatherForecast.Adapt(repo);

        return Ok(repo);
    }

    [HttpGet]
    public IActionResult MappingListObject()
    {
        var weatherForecastDTOs = list.Adapt<IList<WeatherForecast>>();

        return Ok(weatherForecastDTOs);
    }

    [HttpGet]
    public IActionResult MappingQueryable()
    {
        var iq = list.AsQueryable().ProjectToType<WeatherForecastDTO>();

        return Ok(iq.ToList());
    }
}
