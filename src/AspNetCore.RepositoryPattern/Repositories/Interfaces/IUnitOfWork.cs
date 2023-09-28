namespace AspNetCore.RepositoryPattern.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : class;
    IBackpackRepository Backpacks { get; }
    ICharacterRepository Characters { get; }
    IFactionRepository Factions { get; }
    IWeaponRepository Weapons { get; }

    Task CommitAsync();
    Task RollbackAsync();

}
