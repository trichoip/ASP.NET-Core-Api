namespace AspNetCore.SMS.ASPSMSs.Services;

public interface ISmsSender
{
    Task SendSmsAsync(string number, string message);
}
