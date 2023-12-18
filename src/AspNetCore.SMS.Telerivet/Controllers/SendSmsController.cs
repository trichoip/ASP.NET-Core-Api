using Microsoft.AspNetCore.Mvc;
using Telerivet.Client;

namespace AspNetCore.SMS.Telerivet.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SendSmsController : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> Send(string phonenumber, string messages)
    {
        // https://www.telerivet.com
        // đăng nhập tài khoản -> lấy token và ProjectById
        // tài khoản trong github
        // tải app telerivet trên điện thoại -> làm theo hưỡng dẫn

        TelerivetAPI tr = new TelerivetAPI("");
        Project project = tr.InitProjectById("");

        Message sent_msg = await project.SendMessageAsync(Util.Options(
            "content", messages,
            "to_number", phonenumber
        ));

        return Ok();

    }

    [HttpPost("webhook")]
    public async Task<IActionResult> WebHook([FromForm] IFormCollection param)
    {
        return Ok(param);
    }
}
