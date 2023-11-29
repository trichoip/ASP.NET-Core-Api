using AspNetCore.Helpers.Helpers;
using AspNetCore.Pattern.RepositoryPattern.Data;
using AspNetCore.Pattern.RepositoryPattern.Repositories.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetCore.Pattern.RepositoryPattern.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext context;
    private readonly DbSet<T> dbSet;

    public GenericRepository(DataContext _context)
    {
        context = _context;
        dbSet = context.Set<T>();
    }

    public virtual IQueryable<T> Entities => context.Set<T>();

    public virtual async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await dbSet.AddAsync(entity, cancellationToken);

    }

    public virtual async Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbSet.ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbSet.FindAsync(id, cancellationToken);
    }

    public virtual async Task<T?> FindByIdAsync(object?[] index, CancellationToken cancellationToken = default)
    {
        return await dbSet.FindAsync(index, cancellationToken);
    }

    public virtual Task DeleteAsync(T entity)
    {
        dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(T entity)
    {
        //dbSet.Attach(entity);
        dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        return await dbSet.Where(expression).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        string? includes = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        if (includes != null)
        {
            foreach (var include in includes.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<T?> FindOneAsync(
        Expression<Func<T, bool>> expression,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        string? includes = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = dbSet;

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        if (includes != null)
        {
            foreach (var include in includes.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<(int, PaginatedList<T>)> FindAsync(
        int pageIndex = 0,
        int pageSize = 0,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var paginatedList = await PaginatedList<T>.CreateAsync(query, pageIndex, pageSize, cancellationToken);

        return (pageSize, paginatedList);
    }

    public async Task<bool> ExistsByAsync(
        Expression<Func<T, bool>>? expression = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<PaginatedList<TDto>> FindWithPaginationAsync<TDto>(
        IMapper mapper,
        int pageIndex = 0,
        int pageSize = 0,
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        CancellationToken cancellationToken = default) where TDto : class
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }
        //mapper.ProjectTo<TDto>(query);
        return await query.ProjectTo<TDto>(mapper.ConfigurationProvider).PaginatedListAsync(pageIndex, pageSize, cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public Task<IQueryable<T>> FindToIQueryableAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null)
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        return Task.FromResult(query);
    }

    public async Task<T?> FindByAsync(
        Expression<Func<T, bool>> expression,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = dbSet;

        if (includeFunc != null)
        {
            query = includeFunc(query);
        }

        return await query.FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<TDTO?> FindByAsync<TDTO>(
        AutoMapper.IConfigurationProvider configuration,
        Expression<Func<T, bool>> expression,
        CancellationToken cancellationToken = default) where TDTO : class
    {
        IQueryable<T> query = dbSet;

        query = query.Where(expression);

        return await query.ProjectTo<TDTO>(configuration).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<TDTO>> FindAsync<TDTO>(
        AutoMapper.IConfigurationProvider configuration,
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

        return await query.ProjectTo<TDTO>(configuration).ToListAsync(cancellationToken);
    }

    public async Task<PaginatedList<TDTO>> FindAsync<TDTO>(
        AutoMapper.IConfigurationProvider configuration,
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

        return await query.ProjectTo<TDTO>(configuration).PaginatedListAsync(pageIndex, pageSize, cancellationToken);
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