using AspNetCore.Email.Email;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace AspNetCore.Email.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        #region Ctor

        private readonly IEmailSender emailSender;
        public EmailController(IEmailSender _emailSender)
        {
            emailSender = _emailSender;
        }

        #endregion

        #region SendMail
        [HttpGet]
        public IActionResult SendMail(string EmailName)
        {
            MailAddress to = new MailAddress(EmailName);
            MailAddress from = new MailAddress(EmailName);

            MailMessage email = new MailMessage(from, to);
            email.IsBodyHtml = true;
            email.Subject = "Testing out email sending 1";
            email.Body = "<i>Hello all the way from the land of C#</i>";

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 25;
            smtp.Credentials = new NetworkCredential("developermode549@gmail.com", "bzqkvojsevsthkvp");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(email);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Ok(new { smtp });
        }
        #endregion

        #region SendMail2
        [HttpGet]
        public IActionResult SendMail2(string EmailName)
        {
            LinkedResource linkedResource = new LinkedResource("wwwroot/images/a.jpg");
            linkedResource.ContentId = Guid.NewGuid().ToString();
            var html = $"<h1>Hello from</h1><img src=\"cid:" + linkedResource.ContentId + "\"/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(linkedResource);

            MailMessage email = new MailMessage
            {
                From = new MailAddress(EmailName),
                To = { new MailAddress(EmailName) },
                Subject = "Testing out email sending 2",
                Body = "<i>Hello all the way from the land of C#</i>",
                IsBodyHtml = true
            };
            email.Attachments.Add(new Attachment("wwwroot/images/a.jpg", MediaTypeNames.Image.Jpeg));
            email.AlternateViews.Add(alternateView);

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential("developermode549@gmail.com", "bzqkvojsevsthkvp"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,

            };

            try
            {
                smtp.Send(email);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Ok(new { smtp });
        }
        #endregion

        #region SendMail3
        [HttpGet]
        public async Task<IActionResult> SendMail3(string Email = "soroyoy721@wlmycn.com", string Subject = "Subject", string Message = "Message")
        {
            await emailSender.SendEmailAsync(Email, Subject, Message);
            return Ok();
        }
        #endregion

    }
}
