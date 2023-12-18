using AspNetCore.SMS.SpeedSMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.SMS.SpeedSMS.Controllers;
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
        var response = await _smsSender.SendSmsAsync(phonenumber, messages);
        return Ok(response);

    }

    // speedsms ko còn support mms
    [HttpGet("MMS")]
    public async Task<IActionResult> SendMMS(string phonenumber, string messages, string link)
    {
        var response = await _smsSender.SendMmsAsync(phonenumber, messages, link);
        return Ok(response);

    }

    [HttpPost("webhook")]
    public async Task<IActionResult> WebHook(object param)
    {
        return Ok(param);
    }
}
