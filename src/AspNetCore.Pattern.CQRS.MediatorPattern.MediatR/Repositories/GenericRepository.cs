using AspNetCore.Api.Helpers.Helpers;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Data;
using AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetCore.Pattern.CQRS.MediatorPattern.MediatR.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext context;
    private readonly DbSet<T> dbSet;

    public GenericRepository(ApplicationDbContext _context)
    {
        context = _context;
        dbSet = context.Set<T>();
    }

    public virtual async Task<T?> FindByIdAsync(object id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FindAsync(id, cancellationToken);
    }

    public virtual async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await dbSet.AddAsync(entity, cancellationToken);

    }

    public virtual Task DeleteAsync(T entity)
    {
        dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(T entity)
    {
        dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<TDTO?> FindByAsync<TDTO>(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default) where TDTO : class
    {
        IQueryable<T> query = dbSet;

        query = query.Where(expression);

        return await query.ProjectToType<TDTO>().FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<TDTO>> FindAsync<TDTO>(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken cancellationToken = default) where TDTO : class
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ProjectToType<TDTO>().ToListAsync(cancellationToken);
    }

    public async Task<PaginatedList<TDTO>> FindAsync<TDTO>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        CancellationToken cancellationToken = default) where TDTO : class
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ProjectToType<TDTO>().PaginatedListAsync(pageIndex, pageSize, cancellationToken);
    }
}