using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.RepositoryPattern.Repositories;

public class UnitOfWork3 : IUnitOfWork
{
    private readonly DataContext _context;
    private bool disposed = false;

    public ICharacterRepository Characters { get; private set; }
    public IBackpackRepository Backpacks { get; private set; }
    public IFactionRepository Factions { get; private set; }
    public IWeaponRepository Weapons { get; private set; }

    public UnitOfWork3(
        DataContext context,
        ICharacterRepository Characters,
        IBackpackRepository Backpacks,
        IFactionRepository Factions,
        IWeaponRepository Weapons)
    {
        _context = context;
        this.Characters = Characters;
        this.Backpacks = Backpacks;
        this.Factions = Factions;
        this.Weapons = Weapons;
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
