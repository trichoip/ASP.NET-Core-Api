using AspNetCore.Extensions.Repositories;
using AspNetCore.Extensions.Services;
using Microsoft.Extensions.DependencyInjection;

//namespace Microsoft.Extensions.DependencyInjection;
namespace AspNetCore.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {

        services.AddScoped<IUserStore, UserStore>();
        services.AddScoped<IRoleStore, RoleStore>();

        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IRoleManager, RoleManager>();

        return services;
    }

}
