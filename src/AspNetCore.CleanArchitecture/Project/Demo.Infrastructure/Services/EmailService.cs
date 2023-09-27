using AspNetCore.CleanArchitecture.Project.Demo.Application.DTOs.Email;
using AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Services;
using System.Net.Mail;

namespace AspNetCore.CleanArchitecture.Project.Demo.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(EmailRequestDto request)
        {
            var emailClient = new SmtpClient("localhost");
            var message = new MailMessage
            {
                From = new MailAddress(request.From),
                Subject = request.Subject,
                Body = request.Body
            };
            message.To.Add(new MailAddress(request.To));
            await emailClient.SendMailAsync(message);
        }
    }
}
