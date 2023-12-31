﻿using AspNetCore.Api.HandleError.Exceptions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace AspNetCore.Api.HandleError.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly ProblemDetailsFactory _factory;

    public ErrorHandlerMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger,
        ProblemDetailsFactory factory)
    {
        _next = next;
        _logger = logger;
        _factory = factory;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            //await HandleExceptionAsync(context, error);

            if (error is DemoException)
            {
                throw new DemoException("DemoException_AddProblemDetails");
            }
            if (error is Demo2Exception)
            {
                throw new Demo2Exception("Demo2Exception_UseExceptionHandler");
            }

            var response = context.Response;
            response.ContentType = MediaTypeNames.Application.Json;

            //var _factory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            switch (error)
            {
                case AppException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = StatusCodes.Status404NotFound;
                    break;
                default:
                    // unhandled error
                    _logger.LogError(error, error.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var problemDetails = _factory.CreateProblemDetails(
                  httpContext: context,
                  statusCode: response.StatusCode,
                  detail: error.Message);

            var result = JsonSerializer.Serialize(problemDetails);
            await response.WriteAsync(result);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception error)
    {
        throw new Exception("");
    }
}
