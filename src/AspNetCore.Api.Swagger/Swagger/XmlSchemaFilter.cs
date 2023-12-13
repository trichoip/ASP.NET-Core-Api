using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.Api.Swagger.Swagger;

public class XmlSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        if (model.Properties == null) return;

        foreach (var entry in model.Properties)
        {
            var name = entry.Key;
            entry.Value.Xml = new OpenApiXml
            {
                Name = name.Substring(0, 1).ToUpper() + name.Substring(1)
            };
        }
    }
}
