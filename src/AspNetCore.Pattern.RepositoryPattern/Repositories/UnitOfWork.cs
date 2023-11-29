using AspNetCore.Pattern.RepositoryPattern.Data;
using AspNetCore.Pattern.RepositoryPattern.Repositories.Interfaces;
using System.Collections;

namespace AspNetCore.Pattern.RepositoryPattern.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private bool disposed = false;
    private Hashtable _repositories;

    public UnitOfWork(DataContext context)
    {
        _context = context;
        CharacterRepository = new CharacterRepository(_context);
        BackpackRepository = new BackpackRepository(_context);
        FactionRepository = new FactionRepository(_context);
        WeaponRepository = new WeaponRepository(_context);
    }

    public ICharacterRepository CharacterRepository { get; private set; }

    public IBackpackRepository BackpackRepository { get; private set; }

    public IFactionRepository FactionRepository { get; private set; }

    public IWeaponRepository WeaponRepository { get; private set; }

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

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
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
