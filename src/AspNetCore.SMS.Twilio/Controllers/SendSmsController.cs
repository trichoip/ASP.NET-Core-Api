using AspNetCore.SMS.Twilio.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.SMS.Twilio.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SendSmsController : ControllerBase
{
    private readonly ISmsSender _smsSender;

    public SendSmsController(ISmsSender smsSender)
    {
        _smsSender = smsSender;
    }

    [HttpGet]
    public async Task<IActionResult> Send(string phonenumber, string messages)
    {
        // https://www.twilio.com
        // trinmse150418@fpt.edu.vn | developermode549@gmail.com | SK2DF1VCFUFC1EMW535R5ETV
        // developermode549@gmail.com | trinmse150418@fpt.edu.vn | D1PYDDXCHX48SRYJNE6VRS41
        await _smsSender.SendSmsAsync(phonenumber, messages);
        return Ok();

    }
}
