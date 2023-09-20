using AspNetCore.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ResponsesStatusCodeController : ControllerBase
    {

        [HttpPost]
        public IActionResult ResponseResult([Required] StatusCodeResult statusCode, TodoItem todoItem)
        {

            #region problemDetails
            ProblemDetails problemDetails = ProblemDetailsFactory.CreateProblemDetails(
                       httpContext: HttpContext,
                       statusCode: StatusCodes.Status404NotFound,
                       detail: "detail",
                       instance: "instance"
                       // nếu dùng ProblemDetailsFactory.CreateProblemDetails thì không cần truyền title và type
                       // nếu không truyền title thì mặc định title là title mặc định của status code
                       //, title: "title",
                       //type: "type"
                       );
            problemDetails.Extensions.Add(nameof(todoItem.Id), todoItem.Id);
            problemDetails.Extensions.Add("object", new { todoItem });
            #endregion

            #region validationProblemDetails

            ModelState.AddModelError("key", "value");

            ValidationProblemDetails validationProblemDetails =
                    ProblemDetailsFactory.CreateValidationProblemDetails(
                                    httpContext: HttpContext,
                                    modelStateDictionary: ModelState,
                                    statusCode: StatusCodes.Status404NotFound,
                                    detail: "detail",
                                    instance: "instance");
            #endregion

            #region problemDetails2
            // cách này không nên sài vì không tự động tạo giá trị mặc định title và type
            ProblemDetails problemDetails2 = new ProblemDetails
            {
                Detail = "detail",
                Instance = "instance",
                Status = StatusCodes.Status404NotFound,
                Title = "title",
                Type = "type",
                Extensions =
                {
                    { nameof(todoItem.Id), todoItem.Id },
                    { "object", new { todoItem } }
                }
            };
            #endregion

            switch (statusCode)
            {
                case StatusCodeResult.NotFound404:
                    // có thể dùng Problem để thay thẻ NotFound
                    return NotFound(problemDetails); //return new NotFoundObjectResult(problemDetails);
                    return NotFound(validationProblemDetails);
                    return NotFound(todoItem);
                    return NotFound(); // return new NotFoundResult();

                case StatusCodeResult.Unauthorized401:
                    // có thể dùng Problem để thay thẻ Unauthorized
                    return Unauthorized(problemDetails); // return new UnauthorizedObjectResult(problemDetails);
                    return Unauthorized(); // return new UnauthorizedResult();

                case StatusCodeResult.BadRequest400:
                    // có thể dùng Problem để thay thẻ BadRequest
                    return BadRequest(ModelState); // new BadRequestObjectResult(ModelState);
                    return BadRequest(); // return new BadRequestResult();
                    return BadRequest(problemDetails); // new BadRequestObjectResult(problemDetails);
                    return BadRequest(validationProblemDetails);

                // Problem là dành cho custom những status code error
                case StatusCodeResult.Problem:
                    // áp dụng cho tất cả các status code, nên sài cách này
                    return Problem(
                     // nếu không truyền title thì mặc định title là title mặc định của status code
                     //type: "type",
                     //title: "title",
                     detail: "detail",
                     instance: "instance",
                     statusCode: 404);

                    // này là giống Problem
                    return new ObjectResult(problemDetails)
                    {
                        StatusCode = problemDetails.Status
                    };

                // StatusCode là dành cho những status code ok
                case StatusCodeResult.StatusCode:

                    return StatusCode(StatusCodes.Status200OK, todoItem);
                    return StatusCode(StatusCodes.Status200OK); // return new StatusCodeResult(StatusCodes.Status200OK);

                    // này là giống StatusCode
                    return new ObjectResult(todoItem)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };

                case StatusCodeResult.Forbidden403:

                    return Forbid(); // return new ForbidResult();

                case StatusCodeResult.OK200:

                    return Ok(todoItem); // return new OkObjectResult(todoItem);
                    return Ok(problemDetails);
                    return Ok(); // return new OkResult()

                case StatusCodeResult.CreatedAtRoute201:
                    return CreatedAtRoute("Map", new { id = 2 }, todoItem);

                case StatusCodeResult.CreatedAtAction201:

                    return CreatedAtAction("RountMap", "ResponsesStatusCode", new { id = 2 }, new { todoItem });

                // CreatedAtAction là trả về status code 201 Created và Response header location: chứa url đến controller và action tương ứng

                // Response header ->  location: https://localhost:7107/api/WeatherForecast/Get?id=1 
                //return CreatedAtAction(nameof(Get), new { id = 1 }, pet);

                case StatusCodeResult.All:

                    return Accepted(); // return new AcceptedResult();
                    return Conflict(); // return new ConflictResult();
                    return NoContent(); // return new NoContentResult();
                    return ValidationProblem(validationProblemDetails);

                default:
                    return Ok();

            }

        }

        [HttpGet(Name = "Map")]
        public IActionResult RountMap(int id)
        {
            return Ok();
        }
    }

    public enum StatusCodeResult
    {
        NotFound404,
        Unauthorized401,
        BadRequest400,
        Problem,
        Forbidden403,
        StatusCode,
        OK200,
        CreatedAtRoute201,
        CreatedAtAction201,
        All
    }

}