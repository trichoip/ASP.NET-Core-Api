using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.RepositoryPattern.Repositories;

public class UnitOfWork2 : IUnitOfWork
{
    private readonly DataContext _context;
    private bool disposed = false;
    private ICharacterRepository characters;
    private IBackpackRepository backpacks;
    private IFactionRepository factions;
    private IWeaponRepository weapons;

    public UnitOfWork2(DataContext context)
    {
        _context = context;
    }

    public ICharacterRepository Characters
    {
        get
        {
            return characters ??= new CharacterRepository(_context);
        }
    }

    public IBackpackRepository Backpacks
    {
        get
        {
            return backpacks ??= new BackpackRepository(_context);
        }
    }

    public IFactionRepository Factions
    {
        get
        {
            return factions ??= new FactionRepository(_context);
        }
    }

    public IWeaponRepository Weapons
    {
        get
        {
            return weapons ??= new WeaponRepository(_context);
        }
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task RollbackAsync()
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