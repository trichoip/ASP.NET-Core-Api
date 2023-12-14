namespace AspNetCore.RealTime.SignalR.Hubs.Interface;

public interface IOfferHubClient
{
    Task SendOffersToUser(List<string> message);

    // nếu có giá trị trả về thì nó sẽ là invoke
    // await Clients.Client(connectionId).InvokeAsync<string>("GetMessage", CancellationToken.None);
    Task<string> GetMessage();
}
