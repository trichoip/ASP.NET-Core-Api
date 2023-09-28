using AspNetCore.Extensions.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AspNetCore.Extensions.Extensions;

public static class PerformanceMiddlewareBuilderExtensions
{
    public static IApplicationBuilder UsePerformanceMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PerformanceMiddleware>();
    }
}
