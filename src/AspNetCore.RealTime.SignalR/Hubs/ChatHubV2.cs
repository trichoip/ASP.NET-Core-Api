using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AspNetCore.RealTime.SignalR.Hubs;

[Authorize]
public class ChatHubV2 : Hub
{
    private readonly PresenceTracker presenceTracker;

    public ChatHubV2(PresenceTracker presenceTracker)
    {
        this.presenceTracker = presenceTracker;
    }

    public async override Task OnConnectedAsync()
    {
        var result = await presenceTracker.ConnectionOpened(Context.User.Identity.Name);
        if (result.UserJoined)
        {
            await Clients.All.SendAsync("newMessage", "system", $"{Context.User.Identity.Name} joined");
        }

        var currentUsers = await presenceTracker.GetOnlineUsers();
        await Clients.Caller.SendAsync("newMessage", "system", $"Currently online:\n{string.Join("\n", currentUsers)}");

        await base.OnConnectedAsync();
    }

    public async override Task OnDisconnectedAsync(Exception? exception)
    {
        var result = await presenceTracker.ConnectionClosed(Context.User.Identity.Name);
        if (result.UserLeft)
        {
            await Clients.All.SendAsync("newMessage", "system", $"{Context.User.Identity.Name} left");
        }

        await base.OnDisconnectedAsync(exception);
    }

    [Authorize(Roles = "admin")]
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("newMessage", Context.User.Identity.Name, message);
    }

    [HubMethodName("direct-message")]
    public async Task SendToUser(string userId, string message)
    {
        var a = Context.UserIdentifier;
        // để sử dụng Clients.User trong SignalR, người dùng cần được xác thực.
        await Clients.User(userId).SendAsync("newMessage", Context.User.Identity.Name, $"{Context.User.Identity.Name} send {message} to {userId}");
    }
}
