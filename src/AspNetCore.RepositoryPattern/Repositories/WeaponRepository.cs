using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Models;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;

namespace AspNetCore.RepositoryPattern.Repositories;

public class WeaponRepository : GenericRepository<Weapon>, IWeaponRepository
{
    private readonly DataContext context;
    public WeaponRepository(DataContext _context) : base(_context)
    {
        context = _context;
    }

}