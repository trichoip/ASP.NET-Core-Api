using AspNetCore.EntityFramework.Data;
using AspNetCore.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AspNetCore.EntityFramework.Repositories;

public interface IFactionRepository
{
    Task<Faction> FindById(int id);

    Task UpdateAsync(Faction faction);

    Task RemoveAsync(Faction faction);

    Task<Faction> CreateAsync(Faction faction);

    Task AttachAsync(Faction faction);

    Task<IEnumerable<Faction>> FindAll();

    Task<IEnumerable<Faction>> Find(Expression<Func<Faction, bool>> predicate);

}
public class FactionRepository : IFactionRepository
{
    private readonly DataContext _context;
    public FactionRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Faction> FindById(int id)
    {
        //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        //var faction = await _context.Factions.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
        var faction = await _context.FindAsync<Faction>(new object[] { id });
        //_context.Entry(faction).State = EntityState.Detached;
        return faction!;
    }

    public async Task RemoveAsync(Faction faction)
    {
        _context.Remove(faction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Faction faction)
    {
        _context.Update(faction);
        await _context.SaveChangesAsync();
    }

    public async Task AttachAsync(Faction faction)
    {
        _context.Attach(faction);
        await _context.SaveChangesAsync();
    }

    public async Task<Faction> CreateAsync(Faction faction)
    {
        _context.Add(faction);
        await _context.SaveChangesAsync();

        return faction;
    }

    public async Task<IEnumerable<Faction>> FindAll()
    {
        return await _context.Factions.ToListAsync();
    }

    public async Task<IEnumerable<Faction>> Find(Expression<Func<Faction, bool>> predicate)
    {
        return await _context.Factions.Where(predicate).ToListAsync();
    }

}

