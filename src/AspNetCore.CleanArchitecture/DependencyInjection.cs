using AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Services;
using AspNetCore.CleanArchitecture.Services;
using Microsoft.AspNetCore.Mvc;

//namespace Microsoft.Extensions.DependencyInjection;
namespace AspNetCore.CleanArchitecture;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        return services;
    }

}
