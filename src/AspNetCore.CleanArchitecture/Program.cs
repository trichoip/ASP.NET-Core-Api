using AspNetCore.CleanArchitecture;
using AspNetCore.CleanArchitecture.Project.Demo.Application.Extensions;
using AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //await app.InitialiseDatabaseAsync();
}

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//try
//{
//    var context = services.GetRequiredService<ApplicationDbContext>();

//    //if (context.Database.IsSqlServer())
//    //{
//    //    context.Database.Migrate();
//    //}

//    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

//    await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
//    await ApplicationDbContextSeed.SeedSampleDataAsync(context);
//}
//catch (Exception ex)
//{
//    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

//    logger.LogError(ex, "An error occurred while migrating or seeding the database.");

//    throw;
//}
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
