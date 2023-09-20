using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.Swagger.Swagger
{
    public class AcceptLanguageHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            if (operation == null)
            {
                throw new Exception("Invalid operation");
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                In = ParameterLocation.Header,
                Name = "accept-language",
                Description = "pass the locale here: examples like => ar,ar-jo,en,en-gb",
                Schema = new OpenApiSchema
                {
                    Type = "String"
                }
            });

        }
    }
}
