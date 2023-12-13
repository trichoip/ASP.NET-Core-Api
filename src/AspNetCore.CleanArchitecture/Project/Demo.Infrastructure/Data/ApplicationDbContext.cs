using AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Services;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Common.Interfaces;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDomainEventDispatcher dispatcher,
        ICurrentUserService currentUserService,
        IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _dispatcher = dispatcher;
        _httpContextAccessor = httpContextAccessor;
        _currentUserService = currentUserService;
    }

    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Stadium> Stadiums => Set<Stadium>();
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    #region Cau Hinh dispatch va Auditable trong 2 class Interceptors nên không cần cấu hình ở đây
    //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    //{
    //    var entries = ChangeTracker
    //        .Entries()
    //        .Where(e => e.Entity is BaseAuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
    //    foreach (var entityEntry in entries)
    //    {
    //        if (entityEntry.State == EntityState.Added)
    //        {
    //            ((BaseAuditableEntity)entityEntry.Entity).CreatedAt = DateTimeOffset.UtcNow;
    //            ((BaseAuditableEntity)entityEntry.Entity).CreatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Anonymous";
    //        }
    //        else
    //        {
    //            Entry((BaseAuditableEntity)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
    //            Entry((BaseAuditableEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
    //        }
    //        ((BaseAuditableEntity)entityEntry.Entity).ModifiedAt = DateTimeOffset.UtcNow;
    //        ((BaseAuditableEntity)entityEntry.Entity).ModifiedBy = _currentUserService.Id ?? "Anonymous";
    //    }
    //    int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    //    // ignore events if no dispatcher provided
    //    if (_dispatcher == null) return result;
    //    // dispatch events only if save was successful
    //    var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
    //        .Select(e => e.Entity)
    //        .Where(e => e.DomainEvents.Any())
    //        .ToArray();
    //    await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);
    //    return result;
    //}
    //public override int SaveChanges()
    //{
    //    return SaveChangesAsync().GetAwaiter().GetResult();
    //} 
    #endregion
}
