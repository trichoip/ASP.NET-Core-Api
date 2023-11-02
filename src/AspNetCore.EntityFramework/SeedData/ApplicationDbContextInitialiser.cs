using AspNetCore.EntityFramework.Data;
using AspNetCore.EntityFramework.Entities;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.EntityFramework.SeedData;

public static class InitialiserExtensions
{
    public static async Task UseInitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        if (app.Environment.IsDevelopment())
        {
            await initialiser.DeletedDatabaseAsync();
            await initialiser.MigrateAsync();
            await initialiser.SeedAsync();
        }

        if (app.Environment.IsProduction())
        {
            await initialiser.MigrateAsync();
        }
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly DataContext _context;
    public ApplicationDbContextInitialiser(
        ILogger<ApplicationDbContextInitialiser> logger,
        DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task MigrateAsync()
    {
        try
        {
            //_context.Database.IsSqlServer();
            //if (_context.Database.ProviderName == typeof(SqlServerOptionsExtension)!.Assembly.GetName().Name)
            //{
            //    await _context.Database.EnsureDeletedAsync();
            //    await _context.Database.MigrateAsync();
            //}

            await _context.Database.EnsureDeletedAsync();
            await _context.Database.MigrateAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task DeletedDatabaseAsync()
    {
        try
        {
            await _context.Database.EnsureDeletedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        var Backpack = new Faker<Backpack>()
                              .RuleFor(c => c.Description, f => f.Name.JobTitle());

        var Factions = new Faker<Faction>()
                            .RuleFor(c => c.Name, f => f.Name.JobTitle());

        var Weapons = new Faker<Weapon>()
                            .RuleFor(c => c.Name, f => f.Name.JobTitle());

        var Characters = new Faker<Character>()
                            .RuleFor(c => c.Name, f => f.Person.UserName)
                            .RuleFor(c => c.Backpack, Backpack.Generate())
                            .RuleFor(c => c.Factions, Factions.Generate(1))
                            .RuleFor(c => c.Weapons, Weapons.Generate(1))
                            .Generate(1);

        if (!await _context.Characters.AnyAsync())
        {
            await _context.Characters.AddRangeAsync(Characters);

            await _context.SaveChangesAsync();
        }

    }
}
