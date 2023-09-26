using System.Linq.Expressions;

namespace AspNetCore.RepositoryPattern.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> FindByIdAsync(int id);
    Task<T> FindByIdAsync(object?[] index);
    Task<IEnumerable<T>> FindAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);
    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includes = null);

    Task<T> FindOneAsync(
        Expression<Func<T, bool>> expression,
        string includes = null);

    Task UpdateAsync(T entity);
    Task CreateAsync(T entity);
    Task CreateRangeAsync(IEnumerable<T> entities);
    Task RemoveAsync(T entity);
    Task RemoveRangeAsync(IEnumerable<T> entities);

    Task SaveChangesAsync();
}
