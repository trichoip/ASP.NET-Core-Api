using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace AspNetCore.RealTime.SignalR.Hubs;

public class SignalrServer : Hub
{
    // all là tất cả các client đang kết nối đến server
    // khi gữi thì tất cả các client đều nhận được
    public async Task BroadcastMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    // Others là tất cả các client đang kết nối đến server ngoại trừ client gửi request
    // khi gữi thì tất cả các client đều nhận được ngoại trừ client gửi request
    public async Task SendToOthers(string message)
    {
        await Clients.Others.SendAsync("ReceiveMessage", message);
    }

    // Caller là chỉ client gửi request mới nhận được message
    // khi client 1 gửi request thì chỉ client 1 nhận được message , còn client khác như 2,3,.. thì không nhận được
    public async Task SendToCaller(string message)
    {
        //throw new Exception("Error from server");
        await Clients.Caller.SendAsync("ReceiveMessage", GetMessageToSend(message));
    }

    // Client(connectionId) là chỉ client có connectionId mới nhận được message
    public async Task SendToIndividual(string connectionId, string message)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", GetMessageToSend(message));
    }

    // Clients(connectionIds) là chỉ các client có connectionIds mới nhận được message
    public async Task SendToMultipleIndividual(string connectionIds, string message)
    {
        var connectionIdList = connectionIds.Split(",");
        await Clients.Clients(connectionIdList).SendAsync("ReceiveMessage", GetMessageToSend(message));
    }

    // Group(groupName) là chỉ các client join vào group mới nhận được messages
    // lưu ý: client chưa join vào group thì không nhận được messages,
    // nhưng nếu client chưa join đó gữi messages vào group thì tất cả các client trong group đều nhận được
    public async Task SendToGroup(string groupName, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", GetMessageToSend(message));
    }

    // Groups.AddToGroupAsync là thêm client vào group
    public async Task AddUserToGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.SendAsync("ReceiveMessage", $"Current user added to {groupName} group");
        await Clients.Others.SendAsync("ReceiveMessage", $"User {Context.ConnectionId} added to {groupName} group");
    }

    // Groups.RemoveFromGroupAsync là xóa client khỏi group
    public async Task RemoveUserFromGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        await Clients.Caller.SendAsync("ReceiveMessage", $"Current user removed from {groupName} group");
        await Clients.Others.SendAsync("ReceiveMessage", $"User {Context.ConnectionId} removed from {groupName} group");
    }

    // Client streaming là client gữi stream lên server
    public async Task BroadcastStream(IAsyncEnumerable<string> stream)
    {
        await foreach (var item in stream)
        {
            await Task.Delay(1000);
            await Clients.Caller.SendAsync("ReceiveMessage", $"Server received {item}");

        }
    }

    // Server streaming là server gữi stream xuống client
    public async IAsyncEnumerable<string> TriggerStream(
        int jobsCount,
        [EnumeratorCancellation]
        CancellationToken cancellationToken)
    {
        for (var i = 0; i < jobsCount; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            yield return $"Job {i} executed succesfully";
            await Task.Delay(1000, cancellationToken);
        }
    }

    private string GetMessageToSend(string originalMessage)
    {
        return $"User connection id: {Context.ConnectionId}. Message: {originalMessage}";
    }
}
