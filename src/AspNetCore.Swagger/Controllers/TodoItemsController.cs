using AspNetCore.Swagger.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Swagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        TodoItem todoItem = new TodoItem { Id = 1, IsComplete = true, Name = "todo 1" };

        [HttpGet(Name = "GetTodoItemsMethod")] // đặt propertity Name là đặt name cho OperationId swagger | operationId = "GetTodoItemsMethod"
        [ProducesResponseType(typeof(TodoItem), 200)]
        [ProducesResponseType(typeof(IDictionary<string, TodoItem>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<TodoItem>> GetTodoItems()
        {
            return new List<TodoItem>() { todoItem, todoItem };
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoItem), 200)]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        //[Produces(MediaTypeNames.Application.Json)]
        public ActionResult<TodoItem> GetTodoItem(long id)
        {
            return todoItem;
        }

        /// <summary>
        /// Retrieves a specific product by unique id
        /// </summary>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>  
        /// Markdown **syntax** supported.
        /// 
        /// ```
        ///    {
        ///    "Logging": {
        ///     "LogLevel": {
        ///       "Default": "Information",
        ///       "Microsoft.AspNetCore": "Warning"
        ///     }
        ///      },
        ///     "AllowedHosts": "*"
        ///    }
        /// ```
        /// 
        /// </remarks>
        /// <param name="id" example="123">The product id</param>
        /// <response code="200">Product retrieved</response>
        /// <response code="404">Product not found</response>
        /// <response code="500">Oops! Can't lookup your product right now</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(List<TodoItem>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult PutTodoItem(long id, List<TodoItem> todoDTO)
        {
            return Ok(todoDTO);
        }

    }
}
