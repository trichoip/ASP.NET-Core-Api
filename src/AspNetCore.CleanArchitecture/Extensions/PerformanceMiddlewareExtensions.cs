using AspNetCore.CleanArchitecture.Middleware;

namespace AspNetCore.CleanArchitecture.Extensions;

public static class PerformanceMiddlewareExtensions
{
    public static IApplicationBuilder UsePerformanceMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PerformanceMiddleware>();
    }
}
