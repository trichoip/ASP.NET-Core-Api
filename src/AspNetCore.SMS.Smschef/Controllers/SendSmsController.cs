using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace AspNetCore.SMS.Smschef.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SendSmsController : ControllerBase
{

    [HttpGet("smschef")]
    public async Task<IActionResult> SendSmschef(string phonenumber, string messages)
    {
        // https://www.cloud.smschef.com
        // đăng nhập -> lấy secret, fetch device -> lấy device
        // tải app sms-gateway trên điện thoại -> làm theo hưỡng dẫn

        var client = new HttpClient();

        var build = new
        {
            secret = "",
            device = "",

            mode = "devices",
            phone = phonenumber,
            message = messages,
            sim = "1",
            priority = 1
        };

        string queryString = ToQueryString(build);

        var response = await client.PostAsJsonAsync($"https://www.cloud.smschef.com/api/send/sms?{queryString}", new { });

        return Ok(await response.Content.ReadAsStringAsync());

    }

    // textsender y hệt smschef mà cùi hơn
    [HttpGet("textsender")]
    public async Task<IActionResult> SendTextSender(string phonenumber, string messages)
    {
        // http://textsender.zeroapps.in
        // đăng nhập -> lấy secret, fetch device -> lấy device
        // tải app textsender trên điện thoại -> làm theo hưỡng dẫn

        var client = new HttpClient();

        var build = new
        {
            secret = "",
            device = "",

            mode = "devices",
            phone = phonenumber,
            message = messages,
            sim = "1",
            priority = 1
        };

        string queryString = ToQueryString(build);

        var response = await client.PostAsJsonAsync($"http://textsender.zeroapps.in/api/send/sms?{queryString}", new { });

        return Ok(await response.Content.ReadAsStringAsync());

    }

    // khong nhận được webhook tu smschef,textsender
    [HttpPost("webhook")]
    public async Task<IActionResult> WebHook([FromForm] IFormCollection param)
    {
        return Ok(param);
    }

    public static string ToQueryString(object obj)
    {
        var properties = from prop in obj.GetType().GetProperties()
                         where prop.GetValue(obj) != null
                         select $"{prop.Name}={HttpUtility.UrlEncode(prop.GetValue(obj).ToString())}";

        return string.Join("&", properties);
    }

}
