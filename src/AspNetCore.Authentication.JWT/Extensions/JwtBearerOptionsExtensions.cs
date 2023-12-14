using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;

namespace AspNetCore.Authentication.JWT.Extensions;

public static class JwtBearerOptionsExtensions
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

                //throw new ForbiddenAccessException();
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

                //throw new UnauthorizedAccessException("You are not authorized to access this resource");

            },

            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            },

            OnMessageReceived = context =>
            {
                //var path = context.HttpContext.Request.Path;
                //if (path.StartsWithSegments("/learningHub"))
                //{
                //    var accessToken = context.Request.Query["access_token"];
                //    if (!string.IsNullOrWhiteSpace(accessToken))
                //    {
                //        context.Token = accessToken;
                //    }
                //}

                var accessToken = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }

            //OnMessageReceived = context =>
            //{
            //    var accessToken = context.Request.Query["access_token"];

            //    // If the request is for our hub...
            //    var path = context.HttpContext.Request.Path;
            //    if (!string.IsNullOrEmpty(accessToken) &&
            //        (path.StartsWithSegments("/hubs/chat")))
            //    {
            //        // Read the token out of the query string
            //        context.Token = accessToken;
            //    }
            //    return Task.CompletedTask;
            //}
        };
    }
}
