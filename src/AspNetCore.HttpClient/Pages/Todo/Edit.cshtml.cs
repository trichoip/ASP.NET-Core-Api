using AspNetCore.HttpClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AspNetCore.HttpClient.Pages.Todo
{
    public class EditModel : PageModel
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
            //ViewData["CategoryId"] = new SelectList(_contextCategory.GetAll(), "CategoryId", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            using var httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.PostAsync("https://localhost:7181/api/Auth/token", null);
            var json = await response.Content.ReadFromJsonAsync<IDictionary<string, object?>>();
            var jwt = json["jwt"]?.ToString();
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {jwt}");

            using StringContent jsonContent = new(JsonSerializer.Serialize(TodoItem), Encoding.UTF8, "application/json");
            response = await httpClient.PutAsync($"https://localhost:7181/api/TodoItems/{TodoItem.Id}", jsonContent);

            if (response is { StatusCode: HttpStatusCode.NotFound })
            {
                return NotFound();
            }

            if (response is { StatusCode: HttpStatusCode.BadRequest })
            {
                return BadRequest();
            }

            if (response is { StatusCode: HttpStatusCode.Unauthorized })
            {
                return Unauthorized();
            }

            if (response is { StatusCode: HttpStatusCode.Forbidden })
            {
                return Forbid();
            }

            TempData["Message"] = $"TodoItem {TodoItem.Id} updated";
            return RedirectToPage("./Index");
        }
    }
}
