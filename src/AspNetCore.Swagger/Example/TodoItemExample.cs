using AspNetCore.Swagger.Models;
using Swashbuckle.AspNetCore.Filters;

namespace AspNetCore.Swagger.Example
{
    // tự động sinh example cho model TodoItem
    // phải thêm 2 cấu hình sau Program.cs
    // builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());
    //  c.ExampleFilters();

    // nếu 1 class mà được cấu hình IExamplesProvider<T> nhiều lần thì mặc định nó sẽ lấy cuối cùng được cấu hình
    // vì vậy để chọn example thì cấu hìnhn như [SwaggerRequestExample(typeof(TodoItem), typeof(TodoItemExample))]
    // [SwaggerResponseExample(201, typeof(TodoItemExample))] tuong tu
    public class TodoItemExample : IExamplesProvider<TodoItem>
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;

        public TodoItemExample(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _env = env;

        }
        public TodoItem GetExamples()
        {
            return new TodoItem
            {
                Id = 3,
                Name = _env.IsDevelopment() ? "Development" : "Production",
                IsComplete = true,
                Sizes = new List<string> { "1", "2", "3" },
                Cars = new string[] { "car1", "car2" },
                Model = Model.Large
            };
        }
    }

    public class TodoItemResponseExample : IExamplesProvider<TodoItem>
    {
        public TodoItem GetExamples()
        {
            return new TodoItem
            {
                Id = 9,
                Name = "Response",
                IsComplete = true,
                Sizes = new List<string> { "1", "2 Response", "3" },
                Cars = new string[] { "Response", "Response" },
                Model = Model.Large
            };
        }
    }

    public class ListTodoItemExample : IExamplesProvider<List<TodoItem>>
    {
        public List<TodoItem> GetExamples()
        {
            return new List<TodoItem>
            {
               new TodoItem
               {
                    Id = 7,
                    Name = "list",
                    IsComplete = true,
                    Sizes = new List<string> { "1 list", "2 list", "3 list" },
                    Cars = new string[] { "list 1", "list 2" },
                    Model = Model.Small
               },
               new TodoItem
               {
                    Id = 8,
                    Name = "list 2",
                    IsComplete = true,
                    Sizes = new List<string> { "1", "2", "3" },
                    Cars = new string[] { "list 3", "list 4" },
                    Model = Model.Small
               },
            };
        }
    }

    public class TodoItemMultilExample : IMultipleExamplesProvider<TodoItem>
    {
        public IEnumerable<SwaggerExample<TodoItem>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Great Britain",
                "Here's an optional description",
                new TodoItem
                {
                    Id = 11,
                    Name = "list 2",
                    IsComplete = true,
                    Sizes = new List<string> { "1", "2", "3" },
                    Cars = new string[] { "list 3", "list 4" },
                    Model = Model.Small
                }
            );

            yield return SwaggerExample.Create(
               "United States",
               "A minor former colony of Great Britain",
               new TodoItem
               {
                   Id = 12,
                   Name = "list 2",
                   IsComplete = true,
                   Sizes = new List<string> { "1", "2", "3" },
                   Cars = new string[] { "list 3", "list 4" },
                   Model = Model.Small
               }
           );
        }
    }

}
