using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.SMS.HttpSms.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SendSmsController : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> Send(string phonenumber, string messages)
    {
        // https://httpsms.com
        // đăng nhập oauth2 google account deveplopermode -> lấy token
        // tải app httpsms trên điện thoại -> làm theo hưỡng dẫn

        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-api-key", "????"/* Get API Key from https://httpsms.com/settings */);

        var build = new
        {
            from = "+84902047854",
            To = phonenumber,
            Content = messages,
        };

        var response = await client.PostAsJsonAsync("https://api.httpsms.com/v1/messages/send", build);

        return Ok(await response.Content.ReadAsStringAsync());

    }

    [HttpPost("webhook")]
    public async Task<IActionResult> WebHook(object param)
    {
        return Ok(param);
    }
}
