using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace asp.net_core_empty_5._0.SignalR
{
    public class StronglyTypedChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(string user, string message) => await Clients.All.ReceiveMessage(user, message);

        public async Task SendMessageToCaller(string user, string message) => await Clients.Caller.ReceiveMessage(user, message);

        public async Task SendMessageToGroup(string user, string message) => await Clients.Group("SignalR Users").ReceiveMessage(user, message);
    }
}
