using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;
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

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
    {
        return await dbSet.Where(expression).ToListAsync();
    }

    public virtual async Task<T> FindByIdAsync(int id)
    {
        return await dbSet.FindAsync(id);
    }

    public virtual async Task<T> FindByIdAsync(object?[] index)
    {
        return await dbSet.FindAsync(index);
    }

    public virtual async Task RemoveAsync(T entity)
    {
        dbSet.Remove(entity);
    }

    public virtual async Task RemoveRangeAsync(IEnumerable<T> entities)
    {
        dbSet.RemoveRange(entities);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        //dbSet.Attach(entity);
        dbSet.Update(entity);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includes = null)
    {
        IQueryable<T> query = dbSet;

        if (expression != null)
        {
            query = query.Where(expression);
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

    public async Task<T> FindOneAsync(Expression<Func<T, bool>> expression, string includes = null)
    {
        IQueryable<T> query = dbSet;
        if (includes != null)
        {
            foreach (var include in includes.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(include);
            }
        }

        return await query.FirstOrDefaultAsync(expression);
    }
}
