using AspNetCore.HttpClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AspNetCore.HttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpClientTwoController : ControllerBase
    {

        [HttpGet("token")]
        public async Task<String> GetToken()
        {
            using var httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.PostAsync("https://localhost:7181/api/Auth/token", null);
            var json = await response.Content.ReadFromJsonAsync<IDictionary<string, object?>>();
            var jwt = json["jwt"]?.ToString();
            var jwt2 = json.TryGetValue("jwt", out var jwtValue) ? jwtValue?.ToString() : null;
            return jwt2;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            string jwt = await GetToken();

            using var httpClient = new System.Net.Http.HttpClient();

            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {jwt}");

            HttpResponseMessage response = await httpClient.GetAsync("https://localhost:7181/api/TodoItems");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                return new ObjectResult(error)
                {
                    StatusCode = error.Status
                };
            }

            var json = await response.Content.ReadFromJsonAsync<IList<IDictionary<string, object?>>>();

            return Ok(json);
        }

        [HttpPost]
        public async Task<IActionResult> PostTodoItem()
        {
            string jwt = await GetToken();
            using var httpClient = new System.Net.Http.HttpClient();

            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {jwt}");

            using StringContent jsonContent = new(JsonSerializer.Serialize(new
            {
                id = 0,
                name = "write code sample",
                isComplete = false
            }), Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7181/api/TodoItems", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                return new ObjectResult(error)
                {
                    StatusCode = error.Status
                };
            }

            var json = await response.Content.ReadFromJsonAsync<IDictionary<string, object?>>();

            return Ok(json);
        }

        [HttpPut]
        public async Task<IActionResult> PutTodoItem()
        {
            string jwt = await GetToken();
            using var httpClient = new System.Net.Http.HttpClient();

            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {jwt}");

            using HttpResponseMessage response = await httpClient.PutAsJsonAsync("https://localhost:7181/api/TodoItems/1",
                            new TodoItem
                            {
                                Id = 1,
                                Name = "update",
                                IsComplete = false
                            });

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                return new ObjectResult(error)
                {
                    StatusCode = error.Status
                };
            }

            var json = await response.Content.ReadAsStringAsync();

            return Ok(json);
        }
    }
}
