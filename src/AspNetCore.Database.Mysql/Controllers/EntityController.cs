using AspNetCore.Database.Mysql.Data;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Database.Mysql.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EntityController : ControllerBase
{
    private readonly DataContext context;
    public EntityController(DataContext _context)
    {
        context = _context;
    }

    [HttpGet]
    public IActionResult CharactersList()
    {
        return Ok(context.Characters.ToList());
    }

}
