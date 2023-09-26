using System.Threading.Tasks;

namespace asp.net_core_empty_5._0.SignalR
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }
}
