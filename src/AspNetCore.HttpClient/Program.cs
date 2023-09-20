
using AspNetCore.HttpClient.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace AspNetCore.HttpClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddRazorPages();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new()
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>(JwtBearerDefaults.AuthenticationScheme);
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                             builder.Configuration.GetSection("AppSettings:SerectKey").Value!)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnForbidden = async context =>
                        {
                            var httpContext = context.HttpContext;
                            var statusCode = StatusCodes.Status403Forbidden;
                            var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                            var problemDetails = factory.CreateProblemDetails(httpContext: httpContext, statusCode: statusCode);

                            // cách 1:
                            context.Response.ContentType = MediaTypeNames.Application.Json;
                            context.Response.StatusCode = statusCode;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));

                        },

                        OnChallenge = async context =>
                        {
                            context.HandleResponse();

                            context.Response.Headers.Append(HeaderNames.WWWAuthenticate, $@"{context.Options.Challenge} error=""{context.Error}"",error_description=""{context.ErrorDescription}""");

                            var httpContext = context.HttpContext;
                            var statusCode = StatusCodes.Status401Unauthorized;
                            var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                            var problemDetails = factory.CreateProblemDetails(httpContext: httpContext, statusCode: statusCode);

                            // cách 2:
                            var routeData = httpContext.GetRouteData();
                            var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
                            var result = new ObjectResult(problemDetails) { StatusCode = statusCode };
                            await result.ExecuteResultAsync(actionContext);

                        },

                    };
                });

            //builder.Services.AddCors();
            builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.EnablePersistAuthorization());
                app.UseHsts();
            }

            //app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapRazorPages();

            app.Run();
        }
    }
}