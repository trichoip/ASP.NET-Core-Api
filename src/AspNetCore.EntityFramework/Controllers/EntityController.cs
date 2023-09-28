using AspNetCore.EntityFramework.Data;
using AspNetCore.EntityFramework.Entities;
using AspNetCore.EntityFramework.Repositories;
using Bogus;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.EntityFramework.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IFactionRepository factionRepository;
        public EntityController(DataContext _context, IFactionRepository _factionRepository)
        {
            context = _context;
            factionRepository = _factionRepository;
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

        [HttpPut]
        public async Task<IActionResult> UpdateError()
        {
            var id = new Faker().Random.Int(1, 20);
            var faction = await factionRepository.FindById(id);

            var factionNew = new Faction
            {
                Id = faction.Id,
                Name = "Updated"
            };

            await factionRepository.UpdateAsync(factionNew);

            return Ok(id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateError2()
        {
            var id = new Faker().Random.Int(1, 20);

            var factionNew = new Faction
            {
                Id = 100,
                Name = "Updated"
            };

            await factionRepository.UpdateAsync(factionNew);

            return Ok(id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateErrorChangeToAdd()
        {
            var id = new Faker().Random.Int(1, 20);

            var factionNew = new Faction
            {
                //Id = 100,// không có id là nó add
                Name = "Updated"
            };

            await factionRepository.UpdateAsync(factionNew);
            //await factionRepository.AttachAsync(factionNew); này tường tư UpdateAsync khi không có id

            return Ok(id);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteError()
        {

            var id = new Faker().Random.Int(1, 20);
            var faction = await factionRepository.FindById(id);

            var factionNew = new Faction
            {
                Id = faction.Id,
                Name = faction.Name
            };

            await factionRepository.RemoveAsync(factionNew);

            return Ok(id);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteError2()
        {

            var id = new Faker().Random.Int(1, 20);
            var faction = await factionRepository.FindById(id);

            var factionNew = new Faction
            {
                Id = 100,
                Name = faction.Name
            };

            await factionRepository.RemoveAsync(factionNew);

            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateError1()
        {

            var id = new Faker().Random.Int(1, 20);
            var faction = await factionRepository.FindById(id);

            await factionRepository.CreateAsync(faction);

            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateError2()
        {

            var id = new Faker().Random.Int(1, 20);

            var faction2 = await factionRepository.FindById(id);

            var faction = new Faction
            {
                Id = id, // id trùng
                Name = "Created"
            };

            await factionRepository.CreateAsync(faction);

            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateError3()
        {

            var id = new Faker().Random.Int(1, 20);

            var faction = new Faction
            {
                Id = 100,
                Name = "Created"
            };

            await factionRepository.CreateAsync(faction);

            return Ok(id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSuccess()
        {
            var id = new Faker().Random.Int(1, 20);
            var faction = await factionRepository.FindById(id);

            faction.Name = "Updated";

            await factionRepository.UpdateAsync(faction);

            return Ok(id);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSuccess()
        {
            var id = new Faker().Random.Int(1, 20);
            var faction = await factionRepository.FindById(id);

            await factionRepository.RemoveAsync(faction);

            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSuccess()
        {

            var id = new Faker().Random.Int(1, 20);
            var faction = await factionRepository.FindById(id);

            faction.Id = 0;

            await factionRepository.CreateAsync(faction);

            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSuccess2()
        {

            var id = new Faker().Random.Int(1, 20);

            var faction = new Faction
            {
                //Id = 0,
                Name = "Created"
            };

            await factionRepository.CreateAsync(faction);

            return Ok(id);
        }

    }
}
