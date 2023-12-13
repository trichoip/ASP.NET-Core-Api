using Mapster;

namespace AspNetCore.ObjectMapping.Mapster;

public record WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

}
public record WeatherForecastDTO
{
    public DateOnly Date { get; set; }

    //[AdaptIgnore]
    public int TemperatureC { get; set; }

    [AdaptMember(nameof(WeatherForecast.Summary))]
    public string? SummaryNow { get; set; }

}
