using AspNetCore.HttpClient.Models;
using AspNetCore.HttpClient.Record;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AspNetCore.HttpClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpClientController : ControllerBase
    {
        //private readonly string url = "https://jsonplaceholder.typicode.com/todos";
        private readonly string url = "https://localhost:7181";

        private readonly System.Net.Http.HttpClient client;
        public HttpClientController()
        {
            client = new System.Net.Http.HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        [HttpGet("token")]
        public async Task<String> GetToken()
        {
            using var httpClient = new System.Net.Http.HttpClient()
            {
                BaseAddress = new Uri(url)
            };

            #region Cach 1

            var responsePostAsJsonAsync = await httpClient.PostAsJsonAsync<object>("api/Auth/token", null);
            var responseContentPostAsJsonAsync = await responsePostAsJsonAsync.Content.ReadFromJsonAsync<TokenJwt>();

            #endregion

            #region cach 2

            var responseString = await responsePostAsJsonAsync.Content.ReadAsStringAsync();
            var jsonObject = JsonSerializer.Deserialize<JsonDocument>(responseString);
            string jwtValue = null;
            if (jsonObject.RootElement.TryGetProperty("jwt", out var jwtProperty))
            {
                // Lấy giá trị của thuộc tính "jwt"
                jwtValue = jwtProperty.GetString();
            }

            #endregion
            return responseContentPostAsJsonAsync.jwt;
        }

        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            string jwt = await GetToken();

            using var httpClient = new System.Net.Http.HttpClient()
            {
                BaseAddress = new Uri(url)
            };

            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {jwt}");

            HttpResponseMessage response = await httpClient.GetAsync("api/TodoItems");

            #region handle cach 1

            try
            {
                response = await httpClient.GetAsync("api/TodoItems");

                if (response is { StatusCode: >= HttpStatusCode.Unauthorized })
                {
                    throw new HttpRequestException("Something went wrong", inner: null, response.StatusCode);
                }
            }
            catch (HttpRequestException ex) when (ex is { StatusCode: HttpStatusCode.Unauthorized })
            {
                return Problem(statusCode: 402);
            }
            #endregion

            #region handle cach 2
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<IDictionary<string, object?>>();
                var error2 = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                return Ok(error);
                return Problem(statusCode: (int)response.StatusCode,
                               instance: $"{response.RequestMessage?.RequestUri}",
                               detail: $"{response.ReasonPhrase} {response.RequestMessage?.Method} {response.RequestMessage?.RequestUri} {response.RequestMessage?.Version}");
            }
            #endregion

            var htmlText = await response.Content.ReadAsStringAsync();
            var jsonList = await response.Content.ReadFromJsonAsync<IList<TodoItem>>();

            var responseJson = await httpClient.GetFromJsonAsync<IList<TodoItem>>("api/TodoItems");

            var todoItems = JsonSerializer.Deserialize<List<TodoItem>>(htmlText, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            JArray jsonArray = JArray.Parse(htmlText);
            var items = jsonArray.Select(x => new TodoItem
            {
                Id = x["id"].Value<int>(),
                Name = (string)x["name"],
                IsComplete = (bool)x["isComplete"]
            }).ToList();

            //var data = await response.Content.ReadAsStringAsync();
            //dynamic temp1 = JObject.Parse(data);
            //items = temp1.value?.ToObject<IList<TodoItem>>() ?? Array.Empty<TodoItem>();

            dynamic temp = JObject.Parse(jsonArray[0].ToString());
            var todo = new TodoItem
            {
                Id = temp.id,
                Name = temp.name,
                IsComplete = temp.isComplete
            };

            return Ok(new { response.ReasonPhrase, response.Headers, htmlText, jsonList, responseJson, todoItems, items, todo });
        }

        [HttpPost]
        public async Task<IActionResult> PostTodoItem()
        {
            string jwt = await GetToken();
            using var httpClient = new System.Net.Http.HttpClient()
            {
                BaseAddress = new Uri(url)
            };

            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {jwt}");

            using StringContent jsonContent = new(JsonSerializer.Serialize(new
            {
                id = 0,
                name = "write code sample",
                isComplete = false
            }), Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await httpClient.PostAsync("api/TodoItems", jsonContent);

            using HttpResponseMessage responseJson = await httpClient.PostAsJsonAsync(
                    "api/TodoItems",
                    new TodoItem
                    {
                        Id = 0,
                        Name = "Show extensions",
                        IsComplete = true
                    });

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var json = await responseJson.Content.ReadFromJsonAsync<TodoItem>();

            var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri("https://localhost:7181/api/TodoItems");
            httpRequestMessage.Content = jsonContent;

            var responseSendAsync = await httpClient.SendAsync(httpRequestMessage);
            var responseContent = await responseSendAsync.Content.ReadFromJsonAsync<TodoItem>();

            return Ok(new { response.Headers, jsonResponse, json, responseContent });
        }

        [HttpPut]
        public async Task<IActionResult> PutTodoItem()
        {
            string jwt = await GetToken();
            using var httpClient = new System.Net.Http.HttpClient()
            {
                BaseAddress = new Uri(url)
            };
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {jwt}");

            using StringContent jsonContent = new(JsonSerializer.Serialize(new
            {
                id = 1,
                name = "update",
                isComplete = true
            }), Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await httpClient.PutAsync("api/TodoItems/1", jsonContent);

            using HttpResponseMessage responseJson = await httpClient.PutAsJsonAsync("api/TodoItems/2",
                            new TodoItem
                            {
                                Id = 2,
                                Name = "update",
                                IsComplete = false
                            });

            var todo = await responseJson.Content.ReadAsStringAsync();
            var htmlText = await response.Content.ReadAsStringAsync();

            return Ok(new { htmlText, todo });
        }

    }
}
