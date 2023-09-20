using AspNetCore.HttpClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace AspNetCore.HttpClient.Pages.Todo
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public TodoItem TodoItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            using var httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.PostAsync("https://localhost:7181/api/Auth/token", null);
            var json = await response.Content.ReadFromJsonAsync<IDictionary<string, object?>>();
            var jwt = json["jwt"]?.ToString();
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {jwt}");

            response = await httpClient.GetAsync($"https://localhost:7181/api/TodoItems/{id}");

            if (response is { StatusCode: HttpStatusCode.NotFound })
            {
                return NotFound();
            }

            if (response is { StatusCode: HttpStatusCode.Unauthorized })
            {
                return Unauthorized();
            }

            if (response is { StatusCode: HttpStatusCode.Forbidden })
            {
                return Forbid();
            }

            TodoItem = await response.Content.ReadFromJsonAsync<TodoItem>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            using var httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.PostAsync("https://localhost:7181/api/Auth/token", null);
            var json = await response.Content.ReadFromJsonAsync<IDictionary<string, object?>>();
            var jwt = json["jwt"]?.ToString();
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {jwt}");

            response = await httpClient.DeleteAsync($"https://localhost:7181/api/TodoItems/{id}");

            if (response is { StatusCode: HttpStatusCode.NotFound })
            {
                return NotFound();
            }

            if (response is { StatusCode: HttpStatusCode.Unauthorized })
            {
                return Unauthorized();
            }

            if (response is { StatusCode: HttpStatusCode.Forbidden })
            {
                return Forbid();
            }

            TempData["Message"] = $"Todo item {id} deleted successfully!";
            return RedirectToPage("./Index");
        }
    }
}
