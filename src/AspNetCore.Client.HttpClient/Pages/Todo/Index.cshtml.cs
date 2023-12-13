using AspNetCore.Client.HttpClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;

namespace AspNetCore.Client.HttpClient.Pages.Todo;

public class IndexModel : PageModel
{
    public IList<TodoItem> TodoItem { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync()
    {
        using var httpClient = new System.Net.Http.HttpClient();

        var response = await httpClient.PostAsync("https://localhost:7181/api/Auth/token", null);
        var json = await response.Content.ReadFromJsonAsync<IDictionary<string, object?>>();
        var jwt = json["jwt"]?.ToString();
        httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {jwt}");

        response = await httpClient.GetAsync("https://localhost:7181/api/TodoItems");

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
            ModelState.AddModelError(string.Empty, error.Title);
            TodoItem = Array.Empty<TodoItem>();
            return Page();
        }

        TodoItem = await response.Content.ReadFromJsonAsync<IList<TodoItem>>();
        return Page();

    }
}
