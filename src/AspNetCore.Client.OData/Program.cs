using AspNetCore.Client.OData.Data;
using AspNetCore.Client.OData.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Reflection;

namespace AspNetCore.Client.OData;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<MyWorldDbContext>(otp => otp.UseSqlite("Data Source=wwwroot//OdataDB.db"));

        // SetMaxTop(10) là dùng $top <= 10 nếu dùng $top >= 10 là lỗi
        builder.Services.AddControllers().AddOData(options =>
                     options.Select().Filter().OrderBy().Count().Expand().SetMaxTop(100)
                            .AddRouteComponents("odatacount", GetEdmModel()));

        //builder.Services.AddODataQueryFilter();
        static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
            // EmployeeODataTest, EmployeeOData và OData là tên của controller
            // lưu ý: nếu dùng cách này thì tên method phải giống httpmethod là Get, Post, Put, Delete,...
            modelBuilder.EntitySet<Employee>("EmployeeODataTest"); // cách 1.1: ControllerBase
            modelBuilder.EntitySet<Employee>("EmployeeOData"); // cách 1.2: ODataController - cách 1.1 và 1.2 là giống nhau

            modelBuilder.EntitySet<Employee>("OData"); // nếu trong OData mà đặt tên mothod giống httpmethod thì nó giống 2 cái trên
            return modelBuilder.GetEdmModel();
        }

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<MyWorldDbContext>();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
            DbInitializer.Initialize(context);
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}