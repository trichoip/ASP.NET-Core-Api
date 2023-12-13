using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.Api.Swagger.Swagger;

public class TagDescriptionsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags = new List<OpenApiTag> {
        new OpenApiTag { Name = "Products", Description = "Browse/manage the product catalog" },
        new OpenApiTag { Name = "Orders", Description = "Submit orders" }
    };
    }
}
