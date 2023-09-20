using AspNetCore.EntityFramework.Data;
using AspNetCore.EntityFramework.DataSeeding;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.EntityFramework
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbContext"))
                                  .UseLazyLoadingProxies());  // cấu hình lazy loading (Microsoft.EntityFrameworkCore.Proxies) - property relationship phai co virtual

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using (var scope = app.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                    context.Database.Migrate(); // nếu sử dụng Migrate thì phải có file migrations đầu tiên
                    DbInitializer.Initialize(context);
                }
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}