
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AspNetCore.SMS.Twilio.Services;

public class AuthMessageSender : ISmsSender
{
    public SMSoptions Options { get; }
    public AuthMessageSender(IOptions<SMSoptions> optionsAccessor)
    {
        Options = optionsAccessor.Value;
    }
    public Task SendSmsAsync(string number, string message)
    {
        // Plug in your SMS service here to send a text message.
        // Your Account SID from twilio.com/console
        var accountSid = Options.SMSAccountIdentification;
        // Your Auth Token from twilio.com/console
        var authToken = Options.SMSAccountPassword;

        TwilioClient.Init(accountSid, authToken);

        return MessageResource.CreateAsync(
          to: new PhoneNumber(number),
          from: new PhoneNumber(Options.SMSAccountFrom),
          body: message);
    }
}
