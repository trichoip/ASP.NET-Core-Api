using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Utils;

namespace AspNetCore.Email.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailKitController : ControllerBase
    {

        [HttpGet]
        public IActionResult MailKit(string EmailName)
        {
            var bodyBuilder = new BodyBuilder();
            var image = bodyBuilder.LinkedResources.Add("wwwroot/images/a.jpg");
            image.ContentId = MimeUtils.GenerateMessageId();
            bodyBuilder.HtmlBody = string.Format(@"<p>Hey,<br>Just wanted to say hi all the way from the land of C#.<br>-- Code guy</p><br><center><img src=""cid:{0}""/></center>", image.ContentId);
            bodyBuilder.Attachments.Add("wwwroot/images/a.jpg");

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Sender Name", EmailName));
            email.To.Add(new MailboxAddress("Receiver Name", EmailName));
            email.Subject = "Testing out email sending 4";
            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);
                smtp.Authenticate("developermode549@gmail.com", "bzqkvojsevsthkvp");
                smtp.Send(email);
                smtp.Disconnect(true);
            }

            return Ok();
        }
    }
}
