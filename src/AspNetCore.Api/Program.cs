using AspNetCore.Api.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace AspNetCore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll", builder =>
            //    {
            //        builder.AllowAnyOrigin()
            //               .AllowAnyMethod()
            //               .AllowAnyHeader();
            //    });
            //});

            // AddEndpointsApiExplorer là để swagger nhận điện các endpoint của minimal API
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

            builder.Services.AddControllers(options =>
            {
                //options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));

                // khi in ra error thì tên propertie error sẽ format lại theo chuẩn camelCase
                // TemperatureC -> temperatureC | Summary -> summary
                // thuận tiện cho fe lấy data về
                options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());

                //options.ModelMetadataDetailsProviders.Add(new NewtonsoftJsonValidationMetadataProvider());
            })
            .AddJsonOptions(options =>
            {
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                // ........
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseCors("AllowAll");
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

        }

    }
}
