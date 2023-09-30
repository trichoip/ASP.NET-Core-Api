using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;

namespace AspNetCore.RepositoryPattern.Repositories;

public class UnitOfWork4 : IUnitOfWork
{
    private readonly DataContext _context;
    private bool disposed = false;

    public UnitOfWork4(DataContext context)
    {
        _context = context;
    }

    public ICharacterRepository CharacterRepository => CharacterRepository ?? new CharacterRepository(_context);

    public IBackpackRepository BackpackRepository => BackpackRepository ?? new BackpackRepository(_context);

    public IFactionRepository FactionRepository => FactionRepository ?? new FactionRepository(_context);

    public IWeaponRepository WeaponRepository => WeaponRepository ?? new WeaponRepository(_context);

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task RollbackAsync()
    {
        _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        throw new NotImplementedException();
    }

}