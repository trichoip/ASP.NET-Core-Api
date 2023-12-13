using AspNetCore.Api.Helpers.Helpers;
using System.Linq.Expressions;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> FindByIdAsync(object id, CancellationToken cancellationToken = default);
    Task<TDTO?> FindByAsync<TDTO>(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default) where TDTO : class;

    Task<IList<TDTO>> FindAsync<TDTO>(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken cancellationToken = default) where TDTO : class;

    Task<PaginatedList<TDTO>> FindAsync<TDTO>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken cancellationToken = default) where TDTO : class;

    Task UpdateAsync(T entity);
    Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity);
}
