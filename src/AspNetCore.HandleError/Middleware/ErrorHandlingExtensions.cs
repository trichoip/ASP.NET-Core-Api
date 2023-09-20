using AspNetCore.HandleError.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;

namespace AspNetCore.HandleError.Middleware
{
    public static class ErrorHandlingExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;

                // IProblemDetailsService: requied AddProblemDetails() 
                if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exceptionType = exceptionHandlerFeature?.Error;

                    switch (exceptionType)
                    {
                        case Demo2Exception e:
                            context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                            break;
                        default:
                            throw new DemoException("DemoException_AddProblemDetails");
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            break;
                    }

                    await problemDetailsService.WriteAsync(new ProblemDetailsContext
                    {
                        HttpContext = context,
                        ProblemDetails =
                        {
                            //Title = title,
                            //Type = type,
                            Detail = exceptionType?.Message
                        }
                    });
                }
            });
        }

    }
}
