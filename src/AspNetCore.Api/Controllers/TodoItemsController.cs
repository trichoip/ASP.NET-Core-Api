
using AspNetCore.Api.Attributes;
using AspNetCore.Api.Models;
using AspNetCore.Api.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;

namespace AspNetCore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        return await _context.TodoItems
            .Select(x => ItemToDTO(x))
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound();
        }

        return ItemToDTO(todoItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }

        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        todoItem.Name = todoDTO.Name;
        todoItem.IsComplete = todoDTO.IsComplete;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var todoItem = new TodoItem
        {
            IsComplete = todoDTO.IsComplete,
            Name = todoDTO.Name
        };

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(long id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }

    private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
       new TodoItemDTO
       {
           Id = todoItem.Id,
           Name = todoItem.Name,
           IsComplete = todoItem.IsComplete
       };

    [HttpPost("add-image")]
    public async Task<IActionResult> PostImageTodoItem(
       [AllowedFileExtensions(".png", ".jpg", ".jpeg", ".gif")] IFormFile image,
       [FromServices] IWebHostEnvironment _webHostEnvironment)
    {
        var todoItem = new TodoItem();

        // add local image
        var ext = Path.GetExtension(image.FileName).ToLowerInvariant();
        var file = Path.Combine(_webHostEnvironment.WebRootPath, "images", image.FileName);
        using (var fileStream = new FileStream(file, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        // add image to database
        using (var ms = new MemoryStream())
        {
            await image.CopyToAsync(ms);
            todoItem.Image = ms.ToArray();
        }

        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = todoItem.Id },
            ItemToDTO(todoItem));
    }

    [HttpGet("down-image/{id}")]
    public async Task<IActionResult> DownImageTodoItem(long id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        return File(todoItem.Image, MediaTypeNames.Image.Jpeg);
    }
}
