using AspNetCore.EntityFramework.Data;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.EntityFramework.Controllers
{
    [Route("api/[controller]/[action]")]
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
            return Ok(context.Characters
                        // dùng AutoInclude trong ModelBuilder thì không cần Include ở đây
                        // xem cấu hình AutoInclude trong DataContext.cs
                        //.Include(c => c.Weapons)
                        //.Include(c => c.Backpack)
                        //.Include(c => c.Factions)
                        .ToList());
        }

    }
}
