using System.Threading.Tasks;

namespace AspNetCore.Client.MVC.SignalR;

public interface IChatClient
{
    Task ReceiveMessage(string user, string message);
}
