using System.Reflection;

namespace AspNetCore.MappingObject.AutoMapper.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        //services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}
