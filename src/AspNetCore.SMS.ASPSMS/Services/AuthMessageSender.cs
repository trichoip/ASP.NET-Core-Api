using Microsoft.Extensions.Options;

namespace AspNetCore.SMS.ASPSMSs.Services;

public class AuthMessageSender : ISmsSender
{
    public SMSoptions Options { get; }
    public AuthMessageSender(IOptions<SMSoptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;
    }
    public Task SendSmsAsync(string number, string message)
    {
        ASPSMS.SMS SMSSender = new ASPSMS.SMS();

        SMSSender.Userkey = Options.SMSAccountIdentification;
        SMSSender.Password = Options.SMSAccountPassword;
        SMSSender.Originator = Options.SMSAccountFrom;

        SMSSender.AddRecipient(number);
        SMSSender.MessageData = message;
        SMSSender.SendTextSMS();
        return Task.FromResult(0);

    }
}
