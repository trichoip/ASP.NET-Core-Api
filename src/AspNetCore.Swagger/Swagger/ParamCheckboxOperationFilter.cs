using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.Swagger.Swagger
{
    public class ParamCheckboxOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // param keywords
            var keywordParam = operation.Parameters.Where(x => x.Name == "keywords").FirstOrDefault();
            keywordParam.Schema.Default = new OpenApiString("mooon");
            keywordParam.Schema.Enum = new List<IOpenApiAny>
            {
                new OpenApiString("availabale"),
                new OpenApiString("mooon")
            };

            // param keywords3
            var keyword3Param = operation.Parameters.Where(x => x.Name == "keywords3").FirstOrDefault();
            keyword3Param.Schema.Items.Default = new OpenApiString("mooon");
            keyword3Param.Schema.Items.Enum = new List<IOpenApiAny>
            {
                new OpenApiString("availabale"),
                new OpenApiString("mooon")
            };
        }

    }
}
