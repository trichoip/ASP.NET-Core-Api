using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Models;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;

namespace AspNetCore.RepositoryPattern.Repositories;

public class CharacterRepository : GenericRepository<Character>, ICharacterRepository
{

    private readonly DataContext context;
    public CharacterRepository(DataContext _context) : base(_context)
    {
        context = _context;
    }
}