using AspNetCore.HandleError.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AspNetCore.HandleError.Middleware
{
    public class ExceptionHandlingFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var factory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();

            if (context.Exception is HttpResponseException httpResponseException)
            {
                var problemDetails = factory.CreateProblemDetails(
                       httpContext: context.HttpContext,
                       statusCode: 403,
                       detail: httpResponseException.Value?.ToString());

                context.Result = new ObjectResult(problemDetails)
                {
                    StatusCode = httpResponseException.StatusCode
                };

                context.ExceptionHandled = true;
            }
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            return base.OnExceptionAsync(context);
        }
    }
}
