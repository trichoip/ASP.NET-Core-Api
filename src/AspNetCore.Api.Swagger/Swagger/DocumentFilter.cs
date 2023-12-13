using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspNetCore.Api.Swagger.Swagger;

public class DocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // nếu cấu hình ở bên Program.cs mà cấu hình thêm ở bên đây mà cấu hình kiểu setter thì nó sẽ mất ở cấu hình ở bên Program.cs
        // vì vậy nếu đã cấu hình ở bên Program.cs 1 phần rồi thì nên cấu hình thêm ở đây kiểu add thì nó cộng thêm vào cấu hình ở bên Program.cs
        // còn nếu không cấu hình ở bên Program.cs thì cấu hình ở đây kiểu setter cũng được

        #region ExternalDocs
        // bên dưới là cấu hình ExternalDocs không có cấu hình ở bên Program.cs
        // nên cấu hình ở đây kiểu setter cũng được
        swaggerDoc.ExternalDocs = new()
        {
            Description = "Find out more about Swagger",
            Url = new("http://swagger.io"),
            Extensions = null,
        };
        #endregion

        #region Extensions
        // bên dưới là cấu hình Extensions không có cấu hình ở bên Program.cs
        // nên cấu hình ở đây kiểu setter cũng được
        swaggerDoc.Extensions =
            new Dictionary<string, IOpenApiExtension>
            {
                ["x-Null"] = new OpenApiNull(),
                ["x-Object-2"] = new OpenApiObject
                {
                    ["key1"] = new OpenApiBoolean(false),
                    ["key2"] = new OpenApiNull()
                },
            };
        #endregion

        #region Servers
        // do đã cấu hình ở bên Program.cs mà ở đây cấu hình kiểu setter thì nó sẽ ghi đè lên cấu hình Servers ở bên Program.cs
        // làm mất đi cấu hình Servers ở bên Program.cs
        //swaggerDoc.Servers = new List<OpenApiServer>
        //{
        //    new()
        //    {
        //        Url = "http://petstore.swagger.io/v1",
        //        Description = "PetStore Server"
        //    },
        //    new()
        //    {
        //        Url = "http://petstore.swagger.io/v2",
        //        Description = "PetStore Server"
        //    }
        //};

        // nếu muốn add Servers khi đã cấu hình ở Program.cs 1 phần thì phải làm như sau
        swaggerDoc.Servers.Add(new OpenApiServer
        {
            Url = "http://petstore.swagger.io/v3",
            Description = "PetStore Server"
        });
        #endregion

        #region Tags
        swaggerDoc.Tags = new List<OpenApiTag>
        {
            new()
            {
                Name = "TodoItems",
                Description = "Everything about your Pets",
                ExternalDocs = new()
                {
                    Description = "Find out more",
                    Url = new("http://swagger.io"),
                },
            },
            new OpenApiTag
            {
                Name = "Store",
                Description = "Access to Petstore Store",
                ExternalDocs = new()
                {
                    Description = "Find out more",
                    Url = new("http://swagger.io"),
                },
            },
            new OpenApiTag
            {
                Name = "Order",
                Description = "Access to Petstore Order",
                ExternalDocs = new()
                {
                    Description = "Find out more",
                    Url = new("http://swagger.io"),
                },
            }
        };
        #endregion

        #region Components.Schemas
        // Như này thì nó sẽ ghi đè lên những gì đã có ở bên Program.cs
        // Components không nên cấu hình kiểu setter  
        //swaggerDoc.Components = new OpenApiComponents
        //{
        //    Schemas = new Dictionary<string, OpenApiSchema>
        //    {
        //        ["ErrorModel"] = new OpenApiSchema
        //        {
        //            Type = "object",
        //            Properties = new Dictionary<string, OpenApiSchema>
        //            {
        //                ["code"] = new OpenApiSchema
        //                {
        //                    Type = "integer",
        //                    Format = "int32"
        //                },
        //                ["message"] = new OpenApiSchema
        //                {
        //                    Type = "string"
        //                }
        //            }
        //        }
        //    },
        //    SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
        //    {
        //        ["basicAuth"] = new OpenApiSecurityScheme
        //        {
        //            Type = SecuritySchemeType.Http,
        //            Scheme = "basic",
        //            Description = "Basic Authorization header using the Bearer scheme."
        //        }
        //    },
        //    Callbacks = null,
        //    Examples = null,
        //    Extensions = null,
        //    Headers = null,
        //    Links = null,
        //    Parameters = null,
        //    RequestBodies = null,
        //    Responses = null,
        //};

        // Components Schemas nên add như này
        // vì có vài Schemas tự generate tự động
        swaggerDoc.Components.Schemas.Add("ErrorModel", new OpenApiSchema
        {
            Type = "object",
            Properties = new Dictionary<string, OpenApiSchema>
            {
                ["code"] = new OpenApiSchema
                {
                    Type = "integer",
                    Format = "int32"
                },
                ["message"] = new OpenApiSchema
                {
                    Type = "string"
                }
            }
        });
        #endregion

        #region Components.SecuritySchemes
        // SecuritySchemes nếu ở bên Program.cs đã cấu hình thì ở đây nếu muốn add thì nên dùng cách dưới
        swaggerDoc.Components.SecuritySchemes.Add("basicAuth", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "basic",
            Description = "Basic Authorization header using the Bearer scheme."
        });

        // SecuritySchemes còn nếu chưa cấu hình ở bên Program.cs thì cấu hình ở đây kiểu setter cũng được
        //swaggerDoc.Components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
        //{
        //    ["basicAuth"] = new OpenApiSecurityScheme
        //    {
        //        Type = SecuritySchemeType.Http,
        //        Scheme = "basic",
        //        Description = "Basic Authorization header using the Bearer scheme."
        //    }
        //}; 
        #endregion

        #region SecurityRequirements
        // nếu ở Program.cs đã cấu hình thì ở đây cấu hình kiểu setter sẽ mất đi cấu hình ở Program.cs
        // nên đã cấu hình ở program.cs 1 phần thì ở đây cấu hình kiểu add
        // còn nếu chưa cấu hình ở Program.cs thì cấu hình ở đây kiểu setter cũng được
        swaggerDoc.SecurityRequirements.Add(new()
        {
            [new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basicAuth"
                }
            }] = new List<string>()
        });
        #endregion

        #region Info
        //swaggerDoc.Info = new()
        //{
        //    Title = "Swagger - V2",
        //    Description = "_This is `REST API for System`_",
        //    Version = "v9.0",
        //    TermsOfService = new("http://tempuri.org/terms"),
        //    Contact = new()
        //    {
        //        Name = "Joe Developer",
        //        Email = "joe.developer@tempuri.org",
        //        Url = new Uri("http://tempuri.org/joe"),
        //    },
        //    License = new OpenApiLicense
        //    {
        //        Name = "Apache 2.0",
        //        Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html"),
        //    }
        //};
        #endregion

    }
}
