namespace AspNetCore.RealTime.SignalR.Hubs.Interface;

public interface IOfferHubClient
{
    Task SendOffersToUser(List<string> message);
}
