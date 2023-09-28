using AspNetCore.HandleError.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net.Mime;

namespace AspNetCore.HandleError.Extensions
{
    public static class ExceptionHandlerBuilderExtensions
    {
        public static void UseException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;
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
            });
        }

    }
}
