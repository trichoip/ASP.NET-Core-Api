using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.RepositoryPattern.Repositories;

public class UnitOfWork3 : IUnitOfWork
{
    private readonly DataContext _context;
    private bool disposed = false;

    public ICharacterRepository CharacterRepository { get; private set; }
    public IBackpackRepository BackpackRepository { get; private set; }
    public IFactionRepository FactionRepository { get; private set; }
    public IWeaponRepository WeaponRepository { get; private set; }

    public UnitOfWork3(
        DataContext context,
        ICharacterRepository characterRepository,
        IBackpackRepository backpackRepository,
        IFactionRepository factionRepository,
        IWeaponRepository weaponRepository)
    {
        _context = context;
        CharacterRepository = characterRepository;
        BackpackRepository = backpackRepository;
        FactionRepository = factionRepository;
        WeaponRepository = weaponRepository;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
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
