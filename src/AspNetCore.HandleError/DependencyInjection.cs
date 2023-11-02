using AspNetCore.HandleError.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net.Mime;
using System.Text.Json;

namespace AspNetCore.HandleError;

public static class DependencyInjection
{

    public static void UseExceptionApplication(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var _factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                context.Response.ContentType = MediaTypeNames.Application.Json;
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionHandlerFeature?.Error;

                switch (exception)
                {
                    case BadRequestException e:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;
                    case ValidationBadRequestException e:
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        break;
                    case ConflictException e:
                        context.Response.StatusCode = StatusCodes.Status409Conflict;
                        break;
                    case ForbiddenAccessException e:
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        break;
                    case NotFoundException e:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;
                    case UnauthorizedAccessException e:
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        break;
                    case AppException e:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                var problemDetails = _factory.CreateProblemDetails(
                             httpContext: context,
                             statusCode: context.Response.StatusCode,
                             detail: exception?.Message);
                var result = JsonSerializer.Serialize(problemDetails);

                if (exception is ValidationBadRequestException badRequestException)
                {
                    if (badRequestException.ModelState != null)
                    {
                        problemDetails = _factory.CreateValidationProblemDetails(
                              httpContext: context,
                              modelStateDictionary: badRequestException.ModelState,
                              statusCode: context.Response.StatusCode,
                              detail: exception?.Message);
                        result = JsonSerializer.Serialize((ValidationProblemDetails)problemDetails);
                    }
                }

                await context.Response.WriteAsync(result);

            });
        });
    }
}
