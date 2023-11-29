using AspNetCore.Pattern.RepositoryPattern.Data;
using AspNetCore.Pattern.RepositoryPattern.Models;
using AspNetCore.Pattern.RepositoryPattern.Repositories.Interfaces;

namespace AspNetCore.Pattern.RepositoryPattern.Repositories;

public class WeaponRepository : GenericRepository<Weapon>, IWeaponRepository
{
    private readonly DataContext context;
    public WeaponRepository(DataContext _context) : base(_context)
    {
        context = _context;
    }

}