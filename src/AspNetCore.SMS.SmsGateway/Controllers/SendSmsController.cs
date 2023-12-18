using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace AspNetCore.SMS.SmsGateway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SendSmsController : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> Send(string phonenumber, string messages)
    {
        // https://app.sms-gateway.app 
        // https://app.droidsend.com (tuong tu sms-gateway)
        // đăng nhập -> lấy key
        // tải app sms-gateway trên điện thoại -> làm theo hưỡng dẫn

        var client = new HttpClient();

        var build = new
        {
            key = "",

            devices = "0",
            number = phonenumber,
            message = messages,
            type = "sms",
            prioritize = 0
        };
        string queryString = ToQueryString(build);

        var response = await client.GetAsync($"https://app.sms-gateway.app/services/send.php?{queryString}");

        return Ok(await response.Content.ReadAsStringAsync());

    }

    [HttpGet("2")]
    public async Task<IActionResult> Send2(string phonenumber, string messages)
    {
        var message = API.SendSingleMessage(phonenumber, messages);
        return Ok(message);

    }

    public static string ToQueryString(object obj)
    {
        var properties = from prop in obj.GetType().GetProperties()
                         where prop.GetValue(obj) != null
                         select $"{prop.Name}={HttpUtility.UrlEncode(prop.GetValue(obj).ToString())}";

        return string.Join("&", properties);
    }
}
