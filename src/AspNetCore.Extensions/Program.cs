using AspNetCore.Extensions.Repositories;
using AspNetCore.Extensions.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserStore, UserStore>();
builder.Services.AddScoped<IRoleStore, RoleStore>();

builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IRoleManager, RoleManager>();

var app = builder.Build();

app.Run();
