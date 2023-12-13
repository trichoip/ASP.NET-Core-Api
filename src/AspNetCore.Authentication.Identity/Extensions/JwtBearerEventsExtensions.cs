using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace AspNetCore.Authentication.Identity.Extensions;

public static class JwtBearerEventsExtensions
{
    public static void HandleEvents(this JwtBearerOptions options)
    {
        options.Events = new JwtBearerEvents
        {
            OnForbidden = async context =>
            {
                var httpContext = context.HttpContext;
                var statusCode = StatusCodes.Status403Forbidden;
                var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var problemDetails = factory.CreateProblemDetails(httpContext: httpContext, statusCode: statusCode);

                // cách 1:
                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));

            },

            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.Headers.Append(HeaderNames.WWWAuthenticate, $@"{context.Options.Challenge} error=""{context.Error}"",error_description=""{context.ErrorDescription}""");

                var httpContext = context.HttpContext;
                var statusCode = StatusCodes.Status401Unauthorized;
                var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var problemDetails = factory.CreateProblemDetails(httpContext: httpContext, statusCode: statusCode);

                // cách 2:
                var routeData = httpContext.GetRouteData();
                var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
                var result = new ObjectResult(problemDetails) { StatusCode = statusCode };
                await result.ExecuteResultAsync(actionContext);

            },

        };
    }
}
