using AspNetCore.Api.Extensions.Repositories;
using AspNetCore.Api.Extensions.Services;
using Microsoft.Extensions.DependencyInjection;

//namespace Microsoft.Extensions.DependencyInjection;
namespace AspNetCore.Api.Extensions;

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
