using AspNetCore.Api.Extensions.Middleware;
using Microsoft.AspNetCore.Builder;

namespace AspNetCore.Api.Extensions.Extensions;

public static class PerformanceMiddlewareBuilderExtensions
{
    public static IApplicationBuilder UsePerformanceMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PerformanceMiddleware>();
    }
}
