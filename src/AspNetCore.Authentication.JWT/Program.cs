using AspNetCore.Authentication.JWT.Extensions;
using AspNetCore.Authentication.JWT.Services;
using AspNetCore.Authentication.JWT.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Security.Claims;
using System.Text;

namespace AspNetCore.Authentication.JWT;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddHttpContextAccessor();

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                         builder.Configuration.GetSection("AppSettings:SerectKey").Value!)),
                    ValidateIssuerSigningKey = true,

                    ClockSkew = TimeSpan.Zero, // mặc định là 5 phút
                    //ValidateLifetime = false, //  default là true -> false không validate expire time, true validate expire time

                    ValidateIssuer = false,
                    ValidateAudience = false,

                    //ValidateIssuer = true,
                    //ValidIssuer = "",
                    //ValidateAudience = true,
                    //ValidAudience = "",

                    NameClaimType = ClaimTypes.NameIdentifier

                };
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.HandleEvents();

                //options.Events = new JwtBearerEvents
                //{
                //    OnForbidden = async context =>
                //    {
                //        var httpContext = context.HttpContext;
                //        var statusCode = StatusCodes.Status403Forbidden;
                //        var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                //        var problemDetails = factory.CreateProblemDetails(httpContext: httpContext, statusCode: statusCode);
                //        // cách 1:
                //        context.Response.ContentType = MediaTypeNames.Application.Json;
                //        context.Response.StatusCode = statusCode;
                //        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
                //    },
                //    OnChallenge = async context =>
                //    {
                //        context.HandleResponse();
                //        context.Response.Headers.Append(HeaderNames.WWWAuthenticate, $@"{context.Options.Challenge} error=""{context.Error}"",error_description=""{context.ErrorDescription}""");
                //        var httpContext = context.HttpContext;
                //        var statusCode = StatusCodes.Status401Unauthorized;
                //        var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                //        var problemDetails = factory.CreateProblemDetails(httpContext: httpContext, statusCode: statusCode);
                //        // cách 2:
                //        var routeData = httpContext.GetRouteData();
                //        var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
                //        var result = new ObjectResult(problemDetails) { StatusCode = statusCode };
                //        await result.ExecuteResultAsync(actionContext);
                //    },

                //    OnAuthenticationFailed = context =>
                //    {
                //        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                //        {
                //            context.Response.Headers.Add("Token-Expired", "true");
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
            });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.EnablePersistAuthorization());
        }

        app.UseCors(x => x
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

}