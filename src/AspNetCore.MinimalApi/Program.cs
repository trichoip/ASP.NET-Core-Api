
using AspNetCore.MinimalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.MinimalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // minimal API
            app.MapGet("/minimal/todoitems", async (TodoContext db) => await db.TodoItems.ToListAsync());
            app.MapGet("/minimal/todoitems/complete", async (TodoContext db) => await db.TodoItems.Where(t => t.IsComplete).ToListAsync());
            app.MapGet("/minimal/todoitems/{id}", async (long id, TodoContext db) =>
                await db.TodoItems.FindAsync(id)
                    is TodoItem todo
                        ? Results.Ok(todo)
                        : Results.NotFound());
            app.MapPost("/minimal/todoitems", async (TodoItem todo, TodoContext db) =>
            {
                db.TodoItems.Add(todo);
                await db.SaveChangesAsync();
                return Results.Created($"/minimal/todoitems/{todo.Id}", todo);
            });
            app.MapPut("/minimal/todoitems/{id}", async (long id, TodoItem inputTodo, TodoContext db) =>
            {
                var todo = await db.TodoItems.FindAsync(id);
                if (todo is null) return Results.NotFound();
                todo.Name = inputTodo.Name;
                todo.IsComplete = inputTodo.IsComplete;
                await db.SaveChangesAsync();
                return Results.NoContent();
            });
            app.MapDelete("/minimal/todoitems/{id}", async (long id, TodoContext db) =>
            {
                if (await db.TodoItems.FindAsync(id) is TodoItem todo)
                {
                    db.TodoItems.Remove(todo);
                    await db.SaveChangesAsync();
                    return Results.NoContent();
                }

                return Results.NotFound();
            });

            // MapGroup API
            var group = app.MapGroup("/group");
            group.MapGet("/group1", () => "group1");
            group.MapGet("/group2", () => "group2");

            // TypedResults API
            var TypedResults = app.MapGroup("/TypedResults");
            TypedResults.MapPost("/TypedResult1", PostTypedResult);
            TypedResults.MapPut("/TypedResult2", PutTypedResult);

            app.Run();

            static async Task<IResult> PostTypedResult()
            {
                return null;
            }

            static Task PutTypedResult(HttpContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}