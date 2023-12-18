using Microsoft.Extensions.Options;

namespace AspNetCore.SMS.SpeedSMS.Services;

public class AuthMessageSender : ISmsSender
{
    private SMSoptions Options { get; }
    private readonly SpeedSMSAPI api;

    public AuthMessageSender(IOptions<SMSoptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;
        api = new SpeedSMSAPI(Options.AccessToken);
    }
    public async Task<string> SendSmsAsync(string number, string message)
    {
        string userInfo = await api.GetUserInfoAsync();
        Console.WriteLine(userInfo);
        string[] phones = new string[] { number };
        string response = await api.SendSMSAsync(phones, message, 5, Options.Sender);
        Console.WriteLine(response);
        return response;
    }

    // ko còn support mms
    public async Task<string> SendMmsAsync(string number, string message, string link)
    {
        string userInfo = await api.GetUserInfoAsync();
        Console.WriteLine(userInfo);
        string[] phones = new string[] { number };
        string response = await api.SendMMSAsync(phones, message, link, Options.Sender);
        Console.WriteLine(response);
        return response;
    }
}
