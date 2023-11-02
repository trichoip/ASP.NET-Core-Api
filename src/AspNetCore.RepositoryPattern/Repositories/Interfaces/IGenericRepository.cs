using AspNetCore.RepositoryPattern.Helpers;
using AutoMapper;
using System.Linq.Expressions;

namespace AspNetCore.RepositoryPattern.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Entities { get; }
    Task<T?> FindByIdAsync(int id);
    Task<T?> FindByIdAsync(object?[] index);
    Task<IEnumerable<T>> FindAllAsync();

    Task<bool> ExistsByAsync(Expression<Func<T, bool>>? expression = null);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);

    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        string? includes = null);

    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);

    Task<(int, PaginatedList<T>)> FindAsync(
        int pageIndex = 0,
        int pageSize = 0,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);

    Task<PaginatedList<TDto>> FindWithPaginationAsync<TDto>(
        IMapper mapper,
        int pageIndex = 0,
        int pageSize = 0,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null) where TDto : class;

    Task<IQueryable<T>> FindToIQueryableAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);

    Task<T?> FindOneAsync(
        Expression<Func<T, bool>> expression,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        string? includes = null);

    Task<T?> FindByAsync(
        Expression<Func<T, bool>> expression,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null);

    Task UpdateAsync(T entity);
    Task CreateAsync(T entity);
    Task CreateRangeAsync(IEnumerable<T> entities);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
    Task SaveChangesAsync();
}
