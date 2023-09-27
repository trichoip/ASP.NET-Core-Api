using AspNetCore.CleanArchitecture.Project.Demo.Application.DTOs.Email;

namespace AspNetCore.CleanArchitecture.Project.Demo.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequestDto request);
    }
}
