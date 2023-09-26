namespace AspNetCore.RepositoryPattern.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IBackpackRepository Backpacks { get; }
    ICharacterRepository Characters { get; }
    IFactionRepository Factions { get; }
    IWeaponRepository Weapons { get; }

    Task CommitAsync();
    Task RollbackAsync();

}
