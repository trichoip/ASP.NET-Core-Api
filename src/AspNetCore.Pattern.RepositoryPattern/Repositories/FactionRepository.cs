using AspNetCore.Pattern.RepositoryPattern.Data;
using AspNetCore.Pattern.RepositoryPattern.Models;
using AspNetCore.Pattern.RepositoryPattern.Repositories.Interfaces;

namespace AspNetCore.Pattern.RepositoryPattern.Repositories;

public class FactionRepository : GenericRepository<Faction>, IFactionRepository
{

    private readonly DataContext context;
    public FactionRepository(DataContext _context) : base(_context)
    {
        context = _context;
    }
}
