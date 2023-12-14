using AspNetCore.RealTime.SignalR.Hubs.Interface;
using Microsoft.AspNetCore.SignalR;

namespace AspNetCore.RealTime.SignalR.Hubs;

public class OfferHub : Hub<IOfferHubClient>
{
    public async Task<string> WaitForMessage(string connectionId)
    {
        //var message = await Clients.Client(connectionId).InvokeAsync<string>("GetMessage", CancellationToken.None);
        //var message = await Clients.Caller.InvokeAsync<string>("GetMessage", CancellationToken.None);
        //string message = await Clients.Caller.GetMessage();
        string message = await Clients.Client(connectionId).GetMessage();
        return message;
    }

    public async Task<string> ServerReturn(
        string connectionId,
        IHubContext<SignalrServer> context,
        IHttpContextAccessor httpContextAccessor)
    {
        // connectionId phải đến từ SignalrServer
        string message = await context.Clients
            .Client(connectionId)
            .InvokeAsync<string>("GetMessage2", CancellationToken.None);

        return message;
    }
}