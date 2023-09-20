using AspNetCore.Swagger.Example;
using AspNetCore.Swagger.Models;
using AspNetCore.Swagger.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace AspNetCore.Swagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("this is method for controller test", "https://localhost:7055/swagger/index.html")]
    //[Tags("Order")]
    public class TestController : ControllerBase
    {
        TodoItem todoItem = new TodoItem { Id = 1, IsComplete = true, Name = "todo 1" };

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new product",
            Description = "Requires admin privileges",
            OperationId = "CreateProduct", // không cần cũng được
            Tags = new[] { "Store" }
        )]
        [SwaggerOperationFilter(typeof(AcceptLanguageHeaderFilter))]
        // SwaggerResponse
        [SwaggerResponse(201, "The product was created", typeof(TodoItem))]
        [SwaggerResponse(400, "The product data is invalid")]
        // SwaggerResponseHeader
        [SwaggerResponseHeader(StatusCodes.Status201Created, "Location", "string", "Location of the newly created resource")]
        [SwaggerResponseHeader(201, "ETag", "string", "An ETag of the resource")]
        [SwaggerResponseHeader(400, "Retry-After", "integer", "Indicates how long to wait before making a new request.", "int32")]
        [SwaggerResponseHeader(new int[] { 201, 401, 403, 404 }, "CustomHeader", "string", "A custom header")]
        //SwaggerResponseExample  | SwaggerRequestExample
        [SwaggerResponseExample(201, typeof(TodoItemExample))]
        [SwaggerRequestExample(typeof(TodoItem), typeof(TodoItemMultilExample))]
        public ActionResult<TodoItem> PostTodoItem([SwaggerRequestBody("description request body", Required = true)] TodoItem todoDTO)
        {
            return CreatedAtAction("DeleteTodoItem", new { id = todoItem.Id }, todoDTO);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "readAccess")]
        [Authorize(Policy = "writeAccess")]
        [SwaggerOperationFilter(typeof(ParamCheckboxOperationFilter))]
        public IActionResult DeleteTodoItem(long id,
          [SwaggerParameter("description")] string keywords,
                                [FromQuery] string[] keywords3)
        {
            return NoContent();
        }

        [Authorize]
        [HttpGet("{fileName}")]
        [Produces("application/octet-stream", Type = typeof(FileResult))]
        public FileResult GetFile(string fileName)
        {
            return File(new MemoryStream(), "application/octet-stream", fileName);
        }
    }
}
