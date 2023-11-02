using AspNetCore.FluentValidations.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace AspNetCore.FluentValidations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                // ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;
                // này chỉ áp dụng fiel liên quan đến fluent còn field nào k liên quan thì nó không camel case 
                // cho nên phải thêm bên dưới để không liên quan đến fluent thì vẫn camel case
                options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            });

            builder.Services.AddValidators();
            //builder.Services.AddFluentValidation(c => c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
            //builder.Services.AddFluentValidationRulesToSwagger();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}