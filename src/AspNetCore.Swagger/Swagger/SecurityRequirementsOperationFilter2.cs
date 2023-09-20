using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.Swagger.Swagger
{
    public class SecurityRequirementsOperationFilter2 : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            // Policy names map to scopes
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct();

            if (requiredScopes.Any())
            {
                //if (!operation.Responses.ContainsKey("401"))
                //{
                //    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                //}

                //if (!operation.Responses.ContainsKey("403"))
                //{
                //    operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
                //}

                //var oAuthScheme = new OpenApiSecurityScheme
                //{
                //    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                //};

                //operation.Security = new List<OpenApiSecurityRequirement>
                //{
                //    new OpenApiSecurityRequirement
                //    {
                //        [ oAuthScheme ] = requiredScopes.ToList()
                //    }
                //};

                operation.Responses.Add("407", new OpenApiResponse { Description = "Unauthorized" });

                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };

                if (requiredScopes.Any(a => string.IsNullOrEmpty(a))) requiredScopes = Enumerable.Empty<string>();

                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [oAuthScheme] = requiredScopes.ToList()
                });

            }
        }
    }
}
