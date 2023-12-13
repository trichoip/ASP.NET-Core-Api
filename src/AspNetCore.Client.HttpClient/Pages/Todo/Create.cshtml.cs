using AspNetCore.Client.HttpClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AspNetCore.Client.HttpClient.Pages.Todo;

public class CreateModel : PageModel
{
    public IActionResult OnGet()
    {
        //ViewData["CategoryId"] = new SelectList(_contextCategory.GetAll(), "CategoryId", "CategoryName");
        return Page();
    }

    [BindProperty]
    public TodoItem TodoItem { get; set; } = default!;

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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
        response = await httpClient.PostAsync($"https://localhost:7181/api/TodoItems", jsonContent);

        if (response is { StatusCode: HttpStatusCode.Unauthorized })
        {
            return Unauthorized();
        }

        if (response is { StatusCode: HttpStatusCode.Forbidden })
        {
            return Forbid();
        }

        TodoItem = await response.Content.ReadFromJsonAsync<TodoItem>();

        response.Headers.TryGetValues("Location", out var location);

        TempData["Message"] = $"TodoItem {TodoItem.Id} created at {location.FirstOrDefault()}";

        return RedirectToPage("./Index");
    }
}
