using System;
using System.Threading.Tasks;
using asp.net_core_empty_5._0.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace asp.net_core_empty_5._0.SignalR
{
    public class ChatHub : Hub
    {

        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);

        //}
        public async Task SendMessageToCaller(string user, string message) => await Clients.Caller.SendAsync("ReceiveMessage", user, message);

        [HubMethodName("SendMessageToUser")]
        public async Task DirectMessage(string user, string message) => await Clients.User(user).SendAsync("ReceiveMessage", user, message);

        public Task SendMessage(string user, string message, ETransportationSystemContext dbService)
        {
            var userName = dbService.Accounts.FirstOrDefaultAsync(x => x.Username == user).Result.Username;
            return Clients.All.SendAsync("ReceiveMessage", userName, message);
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public Task SendPrivateMessage(string user, string message)
        {
            return Clients.User(user).SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToGroup(string user, string message) => await Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message);

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }
    }
}
