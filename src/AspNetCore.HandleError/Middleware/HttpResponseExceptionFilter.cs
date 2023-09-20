using AspNetCore.HandleError.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AspNetCore.HandleError.Middleware
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var factory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            if (context.Exception is HttpResponseException httpResponseException)
            {
                var problemDetails = factory.CreateProblemDetails(
                       httpContext: context.HttpContext,
                       statusCode: httpResponseException.StatusCode,
                       detail: httpResponseException.Value?.ToString());

                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = httpResponseException.StatusCode
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
