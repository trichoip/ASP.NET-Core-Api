using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Helpers;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetCore.RepositoryPattern.Repositories;

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

    public virtual async Task CreateAsync(T entity)
    {
        await dbSet.AddAsync(entity);

    }

    public virtual async Task CreateRangeAsync(IEnumerable<T> entities)
    {
        await dbSet.AddRangeAsync(entities);
    }

    public virtual async Task<IEnumerable<T>> FindAllAsync()
    {
        return await dbSet.ToListAsync();
    }

    public virtual async Task<T?> FindByIdAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<T?> FindByIdAsync(object?[] index)
    {
        return await dbSet.FindAsync(index);
    }

    public virtual Task RemoveAsync(T entity)
    {
        dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task RemoveRangeAsync(IEnumerable<T> entities)
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

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
    {
        return await dbSet.Where(expression).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>>? expression = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        string? includes = null)
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

        return await query.ToListAsync();
    }

    public async Task<T?> FindOneAsync(
        Expression<Func<T, bool>> expression,
        Func<IQueryable<T>, IQueryable<T>>? includeFunc = null,
        string? includes = null)
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

        return await query.FirstOrDefaultAsync(expression);
    }

    public async Task<(int, PaginatedList<T>)> FindAsync(
        int pageIndex = 0,
        int pageSize = 0,
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

        var paginatedList = await PaginatedList<T>.CreateAsync(query, pageIndex, pageSize);

        return (pageSize, paginatedList);
    }

    public async Task<bool> ExistsByAsync(Expression<Func<T, bool>>? expression = null)
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
        }
        return await query.AnyAsync();
    }

    public async Task<PaginatedList<TDto>> FindWithPaginationAsync<TDto>(
         IMapper mapper,
         int pageIndex = 0,
         int pageSize = 0,
         Expression<Func<T, bool>>? expression = null,
         Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
         Func<IQueryable<T>, IQueryable<T>>? includeFunc = null) where TDto : class
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

        var paginatedList = await query.ProjectTo<TDto>(mapper.ConfigurationProvider).PaginatedListAsync(pageIndex, pageSize);
        return paginatedList;
    }
}

public static class MappingExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int pageNumber, int pageSize) where TDestination : class
      => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
}