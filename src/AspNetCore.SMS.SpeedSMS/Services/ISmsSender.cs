namespace AspNetCore.SMS.SpeedSMS.Services;

public interface ISmsSender
{
    Task<string> SendSmsAsync(string number, string message);
    Task<string> SendMmsAsync(string number, string message, string link);
}
