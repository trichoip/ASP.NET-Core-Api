using AspNetCore.HandleError.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Net.Mime;

namespace AspNetCore.HandleError.Middleware;

public class ExceptionHandler2Middleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public ExceptionHandler2Middleware(
        RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger,
        IProblemDetailsService problemDetailsService)
    {
        _next = next;
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

        }
        catch (Exception error)
        {

            if (error is DemoException)
            {
                throw new DemoException("DemoException_AddProblemDetails");
            }
            if (error is Demo2Exception)
            {
                throw new Demo2Exception("Demo2Exception_UseExceptionHandler");
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;

            // có thể nhùng DI IProblemDetailsService vào đây nhứ ở ctor
            if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
            {
                var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exceptionType = exceptionHandlerFeature?.Error;

                switch (error)
                {
                    case AppException e:
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        break;
                    default:
                        // unhandled error
                        _logger.LogError(error, error.Message);
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
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
        }
    }
}

