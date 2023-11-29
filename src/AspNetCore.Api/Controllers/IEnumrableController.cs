using AspNetCore.Api.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class IEnumrableController : ControllerBase
{
    TodoItemDTO todo = new TodoItemDTO
    {
        Id = 1,
        Name = "Test",
        IsComplete = true,
    };

    List<TodoItemDTO> list;

    public IEnumrableController()
    {
        list = new List<TodoItemDTO>() { todo, todo, todo, todo, todo, todo, todo, todo };
    }

    [HttpGet]
    public IActionResult Get()
    {
        var todolist = list.Where(_ => _.IsComplete == true);

        //foreach (var item in todolist)
        //{
        //    item.IsComplete = false;
        //}

        todolist.ToList().ForEach(_ => _.IsComplete = false);

        return Ok(todolist);
    }
}
