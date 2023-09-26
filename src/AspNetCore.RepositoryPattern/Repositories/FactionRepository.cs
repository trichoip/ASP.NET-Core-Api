using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Models;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;

namespace AspNetCore.RepositoryPattern.Repositories;

public class FactionRepository : GenericRepository<Faction>, IFactionRepository
{

    private readonly DataContext context;
    public FactionRepository(DataContext _context) : base(_context)
    {
        context = _context;
    }
}
