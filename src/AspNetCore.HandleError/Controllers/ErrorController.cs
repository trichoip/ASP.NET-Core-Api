using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.HandleError.Controllers
{

    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        #region Handle error cach 3

        [Route("/error")]
        public IActionResult Error([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exceptionType = context.Error.GetType();

            if (exceptionType == typeof(ArgumentException)
                || exceptionType == typeof(ArgumentNullException)
                || exceptionType == typeof(ArgumentOutOfRangeException))
            {
                if (webHostEnvironment.IsDevelopment())
                {
                    return ValidationProblem(
                        detail: context.Error.StackTrace,
                        title: context.Error.Message);
                }

                return ValidationProblem(detail: context.Error.Message);
            }

            if (exceptionType == typeof(KeyNotFoundException))
            {

                return Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    detail: context.Error.StackTrace,
                    title: context.Error.Message);
                //return NotFound(context.Error.Message);
            }

            if (webHostEnvironment.IsDevelopment())
            {
                return Problem(
                  detail: context.Error.StackTrace,
                  title: context.Error.Message);
            }

            return Problem();
        }
        #endregion

    }
}
