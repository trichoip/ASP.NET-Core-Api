using AspNetCore.Api.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Api.Extensions.Extensions;

public static class IdentityServiceCollectionExtensions
{
    public static void AddIdentity<TUser, TRole>(this IServiceCollection services)
        where TUser : class
        where TRole : class
    {
        services.AddIdentity<TUser, TRole>(setupAction: null!);
    }

    // AddSignInManager
    // AddSignInManagerDeps
    // AddDefaultTokenProviders
    // AddAuthentication

}
