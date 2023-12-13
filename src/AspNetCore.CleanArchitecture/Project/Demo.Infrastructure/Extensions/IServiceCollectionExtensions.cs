using AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Repositories;
using AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Services;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Common;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Common.Interfaces;
using AspNetCore.CleanArchitecture.Project.Demo.Domain.Constants;
using AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Data;
using AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Data.Interceptors;
using AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Repositories;
using AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Extensions;

public static class IServiceCollectionExtensions
{

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext(configuration);
        services.AddRepositories();
        services.AddServices();

        //services.AddScoped<ApplicationDbContextInitialiser>();
        //services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        //services.AddAuthentication()
        //            .AddBearerToken(IdentityConstants.BearerScheme);
    }

    //public static void AddMappings(this IServiceCollection services)
    //{
    //    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    //}

    public static void AddServices(this IServiceCollection services)
    {
        services
            .AddTransient<IMediator, Mediator>()
            .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddTransient<IDateTimeService, DateTimeService>()
            .AddTransient<IEmailService, EmailService>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services
            .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
            .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddTransient<IPlayerRepository, PlayerRepository>()
            .AddTransient<IClubRepository, ClubRepository>()
            .AddTransient<IStadiumRepository, StadiumRepository>()
            .AddTransient<ICountryRepository, CountryRepository>();
    }

    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
           options.UseMySQL(connectionString!, builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                  .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>()));

    }

}
