using AspNetCore.EntityFramework.Data;
using AspNetCore.EntityFramework.Data.Interceptors;
using AspNetCore.EntityFramework.Repositories;
using AspNetCore.EntityFramework.SeedData;
using AspNetCore.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AspNetCore.EntityFramework
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            //builder.Services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //});

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>((sp, options) =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext"), builder =>
                {
                    // default là project chứa DbContext
                    builder.MigrationsAssembly(typeof(DataContext).Assembly.FullName);

                    // áp dụng global SplitQuery 
                    // https://learn.microsoft.com/vi-vn/ef/core/querying/single-split-queries
                    // khi entity có quan hệ 1-n thì sẽ split query
                    // ví dụ như entity father có list children thì sẽ split query ra 2 câu query riêng biệt để tránh duplicate data
                    // 1 câu query lấy father, 1 câu query lấy list children
                    // nếu child có father thì nó không có split query mà nó sẽ join vào câu query lấy child
                    // split query chỉ áp dụng cho list child, bất kỳ list child hoặc trong child có list child nữa thì nó sẽ split query
                    // nếu không muốn split query thì có thể dùng .AsSingleQuery() trong câu query
                    // SplitQuery chi áp dụng cho include hoặc select child (chủ yếu select của ProjectTo automapper)
                    // không áp dụng cho load lazy
                    builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                }).AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                  //.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted }) // log sql command   
                  //.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning))
                  .EnableSensitiveDataLogging()
                  .EnableDetailedErrors()
                  //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // cấu hình no tracking cho context
                  // lưu ý: Lazy loading is not supported for detached entities or entities that are loaded with 'AsNoTracking'
                  .UseLazyLoadingProxies()); // cấu hình lazy loading (Microsoft.EntityFrameworkCore.Proxies) - property relationship phai co virtual

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            builder.Services.AddScoped<ApplicationDbContextInitialiser>();

            builder.Services.AddScoped<IFactionRepository, FactionRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using (var scope = app.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();

                        //if (context.Database.IsSqlServer())
                        //{
                        //    await context.Database.MigrateAsync();
                        //}

                        //if (context.Database.ProviderName == typeof(SqlServerOptionsExtension)!.Assembly.GetName().Name)
                        //{
                        //    await context.Database.EnsureDeletedAsync();
                        //    await context.Database.MigrateAsync();
                        //}

                        await context.Database.EnsureDeletedAsync();
                        await context.Database.MigrateAsync(); // nếu sử dụng Migrate thì phải có file migrations đầu tiên

                        // Seed the database
                        // cách 1: tạo object
                        await DbInitializer.Initialize(context);
                        // cách 2 đọc file json
                        await StoreContextSeed.SeedAsync(context, logger);
                        // cách 3: dùng extension method // cách này cần builder.Services.AddScoped<ApplicationDbContextInitialiser>();
                        await app.UseInitialiseDatabaseAsync();

                    }
                    catch (Exception ex)
                    {
                        var loggerProgram = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                        loggerProgram.LogError(ex, "An error occurred while migrating or seeding the database.");

                        throw;
                    }
                }
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
