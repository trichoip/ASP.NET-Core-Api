using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Extensions.Extensions
{
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
}
