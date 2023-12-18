namespace AspNetCore.SMS.Twilio.Services;

public class SMSoptions
{
    public string SMSAccountIdentification { get; set; } = default!;
    public string SMSAccountPassword { get; set; } = default!;
    public string SMSAccountFrom { get; set; } = default!;
}