using AspNetCore.SMS.ASPSMSs.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.SMS.ASPSMSs.Controllers;
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
        // https://www.aspsms.com/en/
        await _smsSender.SendSmsAsync(phonenumber, messages);
        return Ok();

    }
}
