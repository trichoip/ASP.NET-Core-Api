using AspNetCore.Client.MVC.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AspNetCore.Client.MVC.Controllers;

public class SignalRController : Controller
{
    private readonly IHubContext<ChatHub> _hubContext;
    public SignalRController(IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task<IActionResult> ChatHub()
    {

        // cấu hình:
        // wwwroot => Select unpkg  => "@microsoft/signalr@latest" => expand  dist/browser and select signalr.js and signalr.min.js
        // create Hub class =>  public class nameHub : Hub
        // Program.cs => services.AddSignalR(); => endpoints.MapHub<nameHub>("/nameHub"); ||  app.MapHub<ChatHub>("/chatHub");

        //await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Server", "Hello client");
        //await _hubContext.Clients.All.SendAsync("LoadProducts");

        return View();
    }
}
