using AspNetCore.Pattern.RepositoryPattern.Data;
using AspNetCore.Pattern.RepositoryPattern.Models;
using AspNetCore.Pattern.RepositoryPattern.Repositories.Interfaces;

namespace AspNetCore.Pattern.RepositoryPattern.Repositories;

public class CharacterRepository : GenericRepository<Character>, ICharacterRepository
{

    private readonly DataContext context;
    public CharacterRepository(DataContext _context) : base(_context)
    {
        context = _context;
    }
}