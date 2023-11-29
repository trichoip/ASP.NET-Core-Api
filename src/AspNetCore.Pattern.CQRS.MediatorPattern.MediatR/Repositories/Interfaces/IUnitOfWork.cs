namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<T> Repository<T>() where T : class;
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();

}
