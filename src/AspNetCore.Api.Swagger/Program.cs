using AspNetCore.Api.Swagger.Swagger;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace AspNetCore.Api.Swagger;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            // nếu có áp dụng application/xml mà bị lỗi properties name khi trả về không nhận data thì áp dụng 3 cách sau
            // cách 1: AddJsonOptions - cách này áp dụng cho json và xml - các properties sẽ theo pascal case
            //.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null)
            // cách 2: dùng c.SchemaFilter<XmlSchemaFilter>(); nó sẽ áp dụng cho xml - các properties sẽ theo pascal case
            // cách 3: định nghĩa example - xem trong folder example
            .AddXmlSerializerFormatters();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddFluentValidation(c => c.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
        builder.Services.AddFluentValidationRulesToSwagger();

        builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
        //builder.Services.AddSwaggerExamplesFromAssemblyOf(typeof(MyExample));
        //builder.Services.AddSwaggerExamplesFromAssemblyOf<MyExample>();

        builder.Services.AddSwaggerGen(c =>
        {
            c.ExampleFilters();

            #region SwaggerDoc
            // có thể thêm nhiều SwaggerDoc nhưng phải khác key
            c.SwaggerDoc("v2", new OpenApiInfo { Title = "My API - V2", Version = "v2" });
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Swagger for System - V1",
                    Description = "_This is list of endpoints and documentations of `REST API for System`_",
                    Version = "v9.0",
                    // TermsOfService cần object là uri thì có thể viết như sau TermsOfService = new Uri("url") hoặc TermsOfService = new("url")
                    //  new("url") là cách viết tắt của new Uri("url")
                    TermsOfService = new("http://tempuri.org/terms"),
                    // Contact cần object là OpenApiContact thì có thể viết như sau Contact = new OpenApiContact() hoặc Contact = new()
                    // new() là cách viết tắt của new OpenApiContact()
                    Contact = new()
                    {
                        Name = "Joe Developer",
                        Email = "joe.developer@tempuri.org",
                        Url = new Uri("http://tempuri.org/joe"),
                        // Extensions cần object là IDictionary<string, IOpenApiExtension>
                        // thì có thể viết như sau Extensions = { {string, IOpenApiExtension } , {string, IOpenApiExtension } }
                        // không cần khởi tạo new Dictionary<string, IOpenApiExtension>() nữa
                        Extensions =
                        {
                            { "x-Object", new OpenApiObject
                                {
                                    { "Byte", new OpenApiByte(Guid.NewGuid().ToByteArray()) },
                                    { "Null", new OpenApiNull() },
                                }
                            },
                            { "x-Age", new OpenApiInteger(42) },
                        }
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Apache 2.0",
                        Url = new Uri("http://www.apache.org/licenses/LICENSE-2.0.html"),
                        // Dictionary<string, IOpenApiExtension> có 2 cách tạo đối tượng
                        Extensions =
                        {
                            // [key] = value
                            ["x-Null"] = new OpenApiNull(),
                            ["x-Object"] = new OpenApiObject { { "key1", new OpenApiBoolean(false) }, { "key2", new OpenApiNull() } },
                            ["x-Object-2"] = new OpenApiObject { ["key1"] = new OpenApiBoolean(false), ["key2"] = new OpenApiNull() },

                        }
                    },
                    Extensions = new Dictionary<string, IOpenApiExtension>
                    {
                        // new OpenApiString("String") không thể viết tắt là new("String") được
                        // vì viết new("String") thì nó chỉ hiểu là new IOpenApiExtension("string")
                        // mà không thể cấu hình iterface được mà chỉ cấu hình class implement của IOpenApiExtension thôi
                        { "x-String", new OpenApiString("String") },
                        { "x-Integer", new OpenApiInteger(42) },
                        { "x-Boolean", new OpenApiBoolean(false) },
                        { "x-Double", new OpenApiDouble(1.2E4D) },
                        { "x-Long", new OpenApiLong(15000000000L) },
                        { "x-Float", new OpenApiFloat(35e3F) },
                        { "x-Array", new OpenApiArray { new OpenApiDouble(1.2E4D), new OpenApiLong(15000000000L) , new OpenApiFloat(35e3F) } },
                        { "x-Object", new OpenApiObject { { "key1", new OpenApiBoolean(false) }, { "key2", new OpenApiNull() } } },
                        { "x-Null", new OpenApiNull() },
                        { "x-Byte", new OpenApiByte(Guid.NewGuid().ToByteArray()) },
                        { "x-Date", new OpenApiDate(DateTime.Now) },
                        { "x-DateTime", new OpenApiDateTime(DateTimeOffset.Now) },
                        { "x-Password", new OpenApiPassword("Password") }
                    }

                });
            #endregion

            #region AddServer
            // có thể add nhiểu server
            c.AddServer(new OpenApiServer()
            {
                Url = "https://{HOST}:{PORT}",
                Description = "config server program 1",
                Variables =
                 //new Dictionary<string, OpenApiServerVariable>()
                {
                    { "HOST", new OpenApiServerVariable{ Description="this is host", Enum= { "github.com", "localhost" }, Default="localhost", Extensions=null } },
                    { "PORT", new OpenApiServerVariable() { Description="this is port", Enum= new List<string>{ "8080", "7055"}, Default="7055", Extensions=null } },
                    { "SCHEMA", new () { Description="this is schema", Enum= null , Default="https", Extensions=null } },
                },
                Extensions = null

            });

            c.AddServer(new()
            {
                Url = "https://localhost:7055",
                Description = "config server program 2",
                Variables = null,
                Extensions = null

            });
            #endregion

            #region AddSecurityDefinition
            // có thể add nhiểu AddSecurityDefinition
            c.AddSecurityDefinition("Apikey", new()
            {
                Description = "Enter the JWT token in the format `{Bearer token}`.",
                Name = "Authorization-x",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme. Example: `bearer {token}`",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
            {
                Description = "***Basic Authorization header using the Bearer scheme.***",
                Type = SecuritySchemeType.Http,
                Scheme = "basic"
            });

            // khi có kiểu trả về object rõ ràng thì có thể khởi tạo bằng cách new()
            // new() là cách viết tắt của new OpenApiSecurityScheme()
            OpenApiSecurityScheme CustomSecurityBearer = new()
            {
                Description = "Enter the JWT token in the format `{Bearer token}`.",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer",
                UnresolvedReference = true,
                // Reference là để cấu hình AddSecurityRequirement
                Reference = new OpenApiReference
                {
                    // id phải trùng với id của AddSecurityDefinition của CustomSecurityBearer
                    Id = "CustomBearer",
                    Type = ReferenceType.SecurityScheme
                }
            };

            // ở đây không thể khởi tạo bằng cách new() được vì không có kiểu trả về rõ ràng
            var CustomSecurityApikey = new OpenApiSecurityScheme()
            {
                Description = "Enter the JWT token in the format `{Bearer token}`.",
                Name = "Authorization-y",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
            };

            c.AddSecurityDefinition("CustomApikey", CustomSecurityApikey);
            c.AddSecurityDefinition("CustomBearer", CustomSecurityBearer);

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("/auth-server/connect/authorize", UriKind.Relative),
                        Scopes = new Dictionary<string, string>
                        {
                            { "readAccess", "Access read operations" },
                            { "writeAccess", "Access write operations" }
                        }
                    }
                }
            });

            #endregion

            #region AddSecurityRequirement
            // AddSecurityRequirement golbal
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    // ưu tiên cách này hơn cách
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Apikey",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new [] { "read:Access", "write:Access" }
                    //new string[] { "read:Access", "write:Access" }
                    //new List<string>()
                },
                {
                    new()
                    {
                        Reference = new()
                        {
                            Id = "Basic",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new string[] { }
                }
            });

            c.AddSecurityRequirement(new()
            {
                // không ưu tiên cách này
                [CustomSecurityBearer] = new string[] { },
                [new()
                {
                    Reference = new()
                    {
                        Id = "CustomBearer",
                        Type = ReferenceType.SecurityScheme
                    }
                }] = new string[] { },
            });

            #endregion

            #region Custom
            // thay đổi tên của OperationId là name method
            c.CustomOperationIds(apiDesc =>
            {
                return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
            });

            //c.CustomSchemaIds(type => type.FullName);
            //c.TagActionsBy(api => api.HttpMethod);
            //c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
            #endregion

            #region enable mô tả operation bằng xml 
            // lưu ý: phải có <GenerateDocumentationFile>true</GenerateDocumentationFile> trong <PropertyGroup>
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            #endregion

            #region EnableAnnotations
            c.EnableAnnotations();
            #endregion

            #region Custom Filter

            c.DocumentFilter<DocumentFilter>();
            c.SchemaFilter<XmlSchemaFilter>();
            c.OperationFilter<OperationFilter>();
            c.OperationFilter<AuthResponsesOperationFilter>();
            c.SchemaFilter<AutoRestSchemaFilter>();
            c.OperationFilter<SecurityRequirementsOperationFilter2>();

            //c.DocumentFilter<TagDescriptionsDocumentFilter>();

            c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();   // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization

            // mặc định là includeUnauthorizedAndForbiddenResponses = true và securitySchemaName =  oauth2
            // xem giá trị mặc định thì xem ở ctor của SecurityRequirementsOperationFilter
            c.OperationFilter<SecurityRequirementsOperationFilter>("Bearer");

            // thêm header vào request của tất cả các api, có thể add vào từng api riêng bằng cách dùng anotation [SwaggerOperationFilter(typeof(AddHeaderOperationFilter))]
            //c.OperationFilter<AcceptLanguageHeaderFilter>();
            //c.OperationFilter<AddHeaderOperationFilter>("Authentication", "Authentication for the request", false);

            c.OperationFilter<AddResponseHeadersFilter>(); // [SwaggerResponseHeader]
            #endregion

        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(c =>
            {

                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers.Add(new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" });
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "My UI";

                c.EnableFilter();
                c.EnableDeepLinking();
                c.DisplayRequestDuration();
                c.DisplayOperationId();
                c.EnablePersistAuthorization();

            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}