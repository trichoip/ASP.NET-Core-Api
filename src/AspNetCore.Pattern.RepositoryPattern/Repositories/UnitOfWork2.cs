using AspNetCore.Pattern.RepositoryPattern.Data;
using AspNetCore.Pattern.RepositoryPattern.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Pattern.RepositoryPattern.Repositories;

public class UnitOfWork2 : IUnitOfWork
{
    private readonly DataContext _context;
    private bool disposed = false;
    private ICharacterRepository _characterRepository;
    private IBackpackRepository _backpackRepository;
    private IFactionRepository _factionRepository;
    private IWeaponRepository _weaponRepository;

    public UnitOfWork2(DataContext context)
    {
        _context = context;
    }

    public ICharacterRepository CharacterRepository
    {
        get
        {
            return _characterRepository ??= new CharacterRepository(_context);
        }
    }

    public IBackpackRepository BackpackRepository
    {
        get
        {
            return _backpackRepository ??= new BackpackRepository(_context);
        }
    }

    public IFactionRepository FactionRepository
    {
        get
        {
            return _factionRepository ??= new FactionRepository(_context);
        }
    }

    public IWeaponRepository WeaponRepository
    {
        get
        {
            return _weaponRepository ??= new WeaponRepository(_context);
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task RollbackAsync()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
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