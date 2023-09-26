using System.Reflection;

namespace AspNetCore.Mapper.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}
