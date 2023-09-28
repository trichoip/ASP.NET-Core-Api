﻿using AspNetCore.RepositoryPattern.Data;
using AspNetCore.RepositoryPattern.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.RepositoryPattern.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private bool disposed = false;

    public UnitOfWork(DataContext context)
    {
        _context = context;
        Characters = new CharacterRepository(_context);
        Backpacks = new BackpackRepository(_context);
        Factions = new FactionRepository(_context);
        Weapons = new WeaponRepository(_context);
    }

    public ICharacterRepository Characters { get; private set; }

    public IBackpackRepository Backpacks { get; private set; }

    public IFactionRepository Factions { get; private set; }

    public IWeaponRepository Weapons { get; private set; }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task RollbackAsync()
    {
        foreach (var entry in _context.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

}