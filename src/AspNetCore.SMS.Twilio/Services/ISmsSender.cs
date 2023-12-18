namespace AspNetCore.SMS.Twilio.Services;

public interface ISmsSender
{
    Task SendSmsAsync(string number, string message);
}
