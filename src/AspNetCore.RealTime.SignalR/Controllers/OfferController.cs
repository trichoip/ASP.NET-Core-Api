using AspNetCore.RealTime.SignalR.Hubs;
using AspNetCore.RealTime.SignalR.Hubs.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AspNetCore.RealTime.SignalR.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfferController : ControllerBase
{
    private IHubContext<OfferHub, IOfferHubClient> messageHub;
    public OfferController(IHubContext<OfferHub, IOfferHubClient> _messageHub)
    {
        messageHub = _messageHub;
    }

    [HttpPost]
    public string Get()
    {
        List<string> offers = new List<string>
        {
            "20% Off on IPhone 12",
            "15% Off on HP Pavillion",
            "25% Off on Samsung Smart TV"
        };
        messageHub.Clients.All.SendOffersToUser(offers);
        return "Offers sent successfully to all users!";
    }
}
