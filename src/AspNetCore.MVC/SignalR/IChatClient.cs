using System.Threading.Tasks;

namespace AspNetCore.MVC.SignalR
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }
}
