using System.Threading.Tasks;
using asp.net_core_empty_5._0.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace asp.net_core_empty_5._0.Controllers
{
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
}
