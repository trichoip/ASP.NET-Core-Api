using Mapster;

namespace AspNetCore.MappingObject.Mapster.Config;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<WeatherForecast, WeatherForecastDTO>
           .NewConfig()
           .TwoWays()
           .Map(dest => dest.SummaryNow, src => src.Summary);

        //TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}
