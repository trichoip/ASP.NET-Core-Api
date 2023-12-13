using AspNetCore.Api.HandleError.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net.Mime;
using System.Text.Json;

namespace AspNetCore.Api.HandleError;

public static class DependencyInjection
{

    public static void UseExceptionApplication(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                var _factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionHandlerFeature?.Error;

                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = exception switch
                {
                    BadRequestException => StatusCodes.Status400BadRequest,
                    NotFoundException => StatusCodes.Status404NotFound,
                    ConflictException => StatusCodes.Status409Conflict,
                    ForbiddenAccessException => StatusCodes.Status403Forbidden,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    ValidationBadRequestException => StatusCodes.Status422UnprocessableEntity,
                    _ => StatusCodes.Status500InternalServerError
                };

                var problemDetails = _factory.CreateProblemDetails(
                             httpContext: context,
                             statusCode: context.Response.StatusCode,
                             detail: exception?.Message);
                var result = JsonSerializer.Serialize(problemDetails);

                if (exception is ValidationBadRequestException validationException)
                {
                    if (validationException.ModelState != null)
                    {
                        problemDetails = _factory.CreateValidationProblemDetails(
                              httpContext: context,
                              modelStateDictionary: validationException.ModelState,
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
