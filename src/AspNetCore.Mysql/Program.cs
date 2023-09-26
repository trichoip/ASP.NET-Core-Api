using AspNetCore.Mysql.Data;
using AspNetCore.Mysql.DataSeeding;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Mysql
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // pomelo mysql
            //var serverVersion = new MySqlServerVersion(new Version(8, 0, 31));
            builder.Services.AddDbContext<DataContext>(
                options => options.UseMySql(builder.Configuration.GetConnectionString("DbContextPomelo"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DbContextPomelo")))
                                  .LogTo(Console.WriteLine, LogLevel.Information)
                                  //.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                                  .EnableSensitiveDataLogging()
                                  .EnableDetailedErrors());

            // mysql
            builder.Services.AddDbContext<DataContext>(
                options => options.UseMySQL(builder.Configuration.GetConnectionString("DbContextMySql")!)
                                  .LogTo(Console.WriteLine, LogLevel.Information)
                                  .EnableSensitiveDataLogging()
                                  .EnableDetailedErrors());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                DbInitializer.Initialize(context);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}