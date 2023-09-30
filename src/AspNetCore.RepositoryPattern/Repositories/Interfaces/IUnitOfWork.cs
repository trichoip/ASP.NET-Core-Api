namespace AspNetCore.RepositoryPattern.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : class;
    IBackpackRepository BackpackRepository { get; }
    ICharacterRepository CharacterRepository { get; }
    IFactionRepository FactionRepository { get; }
    IWeaponRepository WeaponRepository { get; }

    Task CommitAsync();
    Task RollbackAsync();

}
