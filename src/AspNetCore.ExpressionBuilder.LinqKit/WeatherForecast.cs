namespace AspNetCore.ExpressionBuilder.LinqKit;

public class WeatherForecast
{
    public int Id { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF { get; set; }

    public string Summary { get; set; }

    public ICollection<Pop> Pops { get; set; }

}

public class Pop
{
    public int Id { get; set; }

    public string? Summary { get; set; }

    public int WeatherForecastId { get; set; }
}
