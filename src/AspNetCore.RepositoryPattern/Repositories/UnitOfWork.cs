using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;
using System.Collections;

namespace AspNetCore.RepositoryPattern.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private bool disposed = false;
    private Hashtable _repositories;

    public UnitOfWork(DataContext context)
    {
        _context = context;
        Characters = new CharacterRepository(_context);
        Backpacks = new BackpackRepository(_context);
        Factions = new FactionRepository(_context);
        Weapons = new WeaponRepository(_context);
    }

    public ICharacterRepository Characters { get; private set; }

    public IBackpackRepository Backpacks { get; private set; }

    public IFactionRepository Factions { get; private set; }

    public IWeaponRepository Weapons { get; private set; }

    public IGenericRepository<T> Repository<T>() where T : class
    {
        if (_repositories == null) _repositories = new Hashtable();

        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);

            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

            _repositories.Add(type, repositoryInstance);
        }

        return (IGenericRepository<T>)_repositories[type];
    }

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

}
