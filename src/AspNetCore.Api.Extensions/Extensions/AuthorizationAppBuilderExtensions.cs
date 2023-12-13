using Microsoft.AspNetCore.Builder;

namespace AspNetCore.Api.Extensions.Extensions;

public static class AuthorizationAppBuilderExtensions
{
    public static IApplicationBuilder UseAuthorization(this IApplicationBuilder app)
    {

        return app;
    }
    // UseHttpsRedirection
    // UseStaticFiles
    // UseAuthentication
    // UseAuthorization

}
