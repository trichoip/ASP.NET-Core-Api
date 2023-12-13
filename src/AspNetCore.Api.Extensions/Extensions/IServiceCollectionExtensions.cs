using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Api.Extensions.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper();
        services.AddMediator();
        services.AddValidators();
        services.AddInitialiseDatabase();
    }

    private static void AddAutoMapper(this IServiceCollection services)
    {
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }

    private static void AddMediator(this IServiceCollection services)
    {
        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }

    private static void AddValidators(this IServiceCollection services)
    {
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddRepositories();
        services.AddServices();

    }

    public static void AddServices(this IServiceCollection services)
    {
        //services
        //    .AddTransient<IMediator, Mediator>()
        //    .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
        //    .AddTransient<IDateTimeService, DateTimeService>()
        //    .AddTransient<IEmailService, EmailService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        //services
        //    .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
        //    .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
        //    .AddTransient<IPlayerRepository, PlayerRepository>()
        //    .AddTransient<IClubRepository, ClubRepository>()
        //    .AddTransient<IStadiumRepository, StadiumRepository>()
        //    .AddTransient<ICountryRepository, CountryRepository>();
    }

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        //services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        //var connectionString = configuration.GetConnectionString("DefaultConnection");

        //services.AddDbContext<ApplicationDbContext>((sp, options) =>
        //   options.UseMySQL(connectionString!,
        //       builder =>
        //       {
        //           builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        //           options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        //       }));

    }

    public static void AddInitialiseDatabase(this IServiceCollection services)
    {
        //services.AddScoped<ApplicationDbContextInitialiser>();
    }

    public static async Task UseInitialiseDatabaseAsync(this WebApplication app)
    {
        //using var scope = app.Services.CreateScope();
        //var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
        //await initialiser.InitialiseAsync();
        //await initialiser.SeedAsync();
    }
}
