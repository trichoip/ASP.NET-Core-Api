using AspNetCore.Pattern.RepositoryPattern.Models;
using AspNetCore.Pattern.RepositoryPattern.Repositories.Interfaces;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Pattern.RepositoryPattern.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CharacterController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICharacterRepository _characterRepository;
    private readonly IBackpackRepository _backpackRepository;

    public CharacterController(
        IUnitOfWork unitOfWork,
        ICharacterRepository characterRepository,
        IBackpackRepository backpackRepository)
    {
        _unitOfWork = unitOfWork;
        _characterRepository = characterRepository;
        _backpackRepository = backpackRepository;

    }

    [HttpGet]
    public async Task<IActionResult> DemoIGenericRepository(CancellationToken cancellationToken)
    {
        var a = await _characterRepository.FindAsync(c => c.Id == 2, cancellationToken);
        var a2 = await _characterRepository.FindAsync(
            null,
            c => c.OrderByDescending(c => c.Id),
            c => c.Include(c => c.Backpack),
            cancellationToken);

        var a3 = await _characterRepository.FindAsync(
            2, 1,
           null,
           c => c.OrderByDescending(c => c.Id),
           c => c.Include(c => c.Backpack));

        return Ok(new { a, a2, a3.Item2 });
    }

    [HttpGet]
    public async Task<IActionResult> SaveCharacterRollback()
    {
        var Backpack = new Faker<Backpack>()
                            .RuleFor(c => c.Description, f => f.Name.JobTitle());
        var Factions = new Faker<Faction>()
                            .RuleFor(c => c.Name, f => f.Name.JobTitle());
        var Weapons = new Faker<Weapon>()
                            .RuleFor(c => c.Name, f => f.Name.JobTitle());
        var Characters = new Faker<Character>()
                            .RuleFor(c => c.Name, f => f.Person.UserName)
                            .RuleFor(c => c.Backpack, Backpack.Generate())
                            .RuleFor(c => c.Factions, Factions.Generate(20))
                            .RuleFor(c => c.Weapons, Weapons.Generate(10))
                            .Generate(2);
        var Backpack2 = new Faker<Backpack>()
            .RuleFor(c => c.Description, f => f.Name.JobTitle()).Generate();
        Backpack2.CharacterId = 1;

        // nếu áp dụng _unitOfWork thì nó áp dụng 1 transaction cho tất cả các repository
        // ví dụ như nếu có 1 repository bị lỗi thì tất cả các repository khác cũng rollback
        // như bên dưới thì khi save CharacterRepository lúc nào cũng thành công
        // còn khi save BackpackRepository thì chỉ thành công lần đầu tiên , còn lần 2 thì bị lỗi
        // thì khi save lần đầu thì nó sẽ save thành công cả 2 và lưu được data trên db
        // còn khi save lần 2 thì nó save thành công CharacterRepository nhưng thất bại save BackpackRepository,
        // vì CharacterRepository và BackpackRepository đều chung 1 dbcontext nên nó chung 1 transaction, mà khi transaction lỗi thì nó sẽ rollback lại kể cả khi save CharacterRepository thành công
        // test hàm này chạy 2 lần là biết, lần đầu sẽ save được cả CharacterRepository và BackpackRepository lên db
        // còn lần 2 thì không có data nào được save lên db vì BackpackRepository lỗi, nó rollback lại cả CharacterRepository

        await _unitOfWork.CharacterRepository.CreateRangeAsync(Characters);
        await _unitOfWork.BackpackRepository.CreateAsync(Backpack2);

        await _unitOfWork.CommitAsync();

        return Ok(Characters);
    }

    [HttpGet]
    public async Task<IActionResult> SaveCharacterNotRollback()
    {
        var Backpack = new Faker<Backpack>()
                          .RuleFor(c => c.Description, f => f.Name.JobTitle());
        var Factions = new Faker<Faction>()
                            .RuleFor(c => c.Name, f => f.Name.JobTitle());
        var Weapons = new Faker<Weapon>()
                            .RuleFor(c => c.Name, f => f.Name.JobTitle());
        var Characters = new Faker<Character>()
                            .RuleFor(c => c.Name, f => f.Person.UserName)
                            .RuleFor(c => c.Backpack, Backpack.Generate())
                            .RuleFor(c => c.Factions, Factions.Generate(20))
                            .RuleFor(c => c.Weapons, Weapons.Generate(10))
                            .Generate(2);
        var Backpack2 = new Faker<Backpack>()
            .RuleFor(c => c.Description, f => f.Name.JobTitle()).Generate();
        Backpack2.CharacterId = 1;

        await _characterRepository.CreateRangeAsync(Characters);
        await _characterRepository.SaveChangesAsync();

        await _backpackRepository.CreateAsync(Backpack2);
        await _backpackRepository.SaveChangesAsync();

        //await _unitOfWork.CommitAsync();
        return Ok(Characters);
    }

}
