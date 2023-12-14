using MessagePack;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Channels;

namespace AspNetCore.RealTime.SignalR.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class ClientController : ControllerBase
{
    static readonly HttpClient httpClient = new HttpClient();
    private readonly string baseUrl;

    public ClientController(IHttpContextAccessor httpContextAccessor)
    {
        var scheme = httpContextAccessor.HttpContext?.Request.Scheme;
        var port = httpContextAccessor.HttpContext?.Connection.LocalPort;
        baseUrl = $"{scheme}://localhost:{port}";
        Console.WriteLine($"baseUrl: {baseUrl}");
    }

    [HttpGet]
    public async Task<IActionResult> Get(string username, string password)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/chatHubV2", HttpTransportType.WebSockets, options =>
        {
            options.AccessTokenProvider = async () =>
            {
                var stringData = JsonSerializer.Serialize(new { username, password });
                var content = new StringContent(stringData);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await httpClient.PostAsync($"{baseUrl}/api/token", content);
                response.EnsureSuccessStatusCode();
                var token = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Token: {token}");
                return token;
            };

            options.HttpMessageHandlerFactory = null;
            options.Headers["CustomData"] = "value";
            options.SkipNegotiation = true;
            options.ApplicationMaxBufferSize = 1_000_000;
            options.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            options.CloseTimeout = TimeSpan.FromSeconds(5);
            options.Cookies = new System.Net.CookieContainer();
            options.DefaultTransferFormat = TransferFormat.Text;
            options.Credentials = null;
            options.Proxy = null;
            options.UseDefaultCredentials = true;
            options.TransportMaxBufferSize = 1_000_000;
            options.WebSocketConfiguration = null;
            options.WebSocketFactory = null;
        })
        .ConfigureLogging(logging =>
        {
            logging.SetMinimumLevel(LogLevel.Information);
            logging.AddConsole();
        })
        .AddMessagePackProtocol(options =>
        {
            options.SerializerOptions = MessagePackSerializerOptions.Standard
               .WithSecurity(MessagePackSecurity.UntrustedData)
               .WithCompression(MessagePackCompression.Lz4Block)
               .WithAllowAssemblyVersionMismatch(true)
               .WithOldSpec()
               .WithOmitAssemblyVersion(true);
        })
        .WithAutomaticReconnect()
        .Build();

        hubConnection.HandshakeTimeout = TimeSpan.FromSeconds(15);
        hubConnection.ServerTimeout = TimeSpan.FromSeconds(30);
        hubConnection.KeepAliveInterval = TimeSpan.FromSeconds(10);

        hubConnection.On<string, string>("newMessage", (sender, message) => Console.WriteLine($"{sender}: {message}"));

        await hubConnection.StartAsync();

        Console.WriteLine("\nConnected!");

        await hubConnection.InvokeAsync("SendMessage", "InvokeAsync InvokeAsync");

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ClientStreaming([FromQuery] string[] messages)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/signalrServer")
            .ConfigureLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Information);
                logging.AddConsole();
            })
            .WithAutomaticReconnect()
            .Build();

        hubConnection.On<string>("ReceiveMessage", message => Console.WriteLine($"SignalR Hub Message: {message}"));

        await hubConnection.StartAsync();
        Console.WriteLine("\nConnected!");

        var channel = Channel.CreateBounded<string>(10);
        await hubConnection.SendAsync("BroadcastStream", channel.Reader);

        foreach (var item in messages)
        {
            await channel.Writer.WriteAsync(item);
        }

        channel.Writer.Complete();

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ServerStreaming(int numberOfJobs, CancellationToken cancellationToken)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/signalrServer")
            .ConfigureLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Information);
                logging.AddConsole();
            })
            .WithAutomaticReconnect()
            .Build();

        hubConnection.On<string>("ReceiveMessage", message => Console.WriteLine($"SignalR Hub Message: {message}"));

        await hubConnection.StartAsync();
        Console.WriteLine("\nConnected!");

        var stream = hubConnection.StreamAsync<string>("TriggerStream", numberOfJobs, cancellationToken);

        await foreach (var reply in stream)
        {
            Console.WriteLine(reply);
        }

        return Ok();
    }
}
