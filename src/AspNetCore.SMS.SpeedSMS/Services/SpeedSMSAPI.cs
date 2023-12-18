using System.Net.Http.Headers;
using System.Text;

namespace AspNetCore.SMS.SpeedSMS.Services;
public class SpeedSMSAPI
{
    public const int TYPE_QC = 1;
    public const int TYPE_CSKH = 2;
    public const int TYPE_BRANDNAME = 3;
    public const int TYPE_BRANDNAME_NOTIFY = 4; // Gửi sms sử dụng brandname Notify
    public const int TYPE_GATEWAY = 5; // Gửi sms sử dụng app android từ số di động cá nhân, download app tại đây: https://speedsms.vn/sms-gateway-service/

    const string rootURL = "https://api.speedsms.vn/index.php";

    private readonly HttpClient _client;

    public SpeedSMSAPI(string accessToken)
    {
        _client = new HttpClient();
        var basicAuthenticationValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{accessToken}:x"));
        _client.DefaultRequestHeaders.Add("Authorization", $"Basic {basicAuthenticationValue}");
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> GetUserInfoAsync()
    {
        string url = $"{rootURL}/user/info";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> SendSMSAsync(string[] phones, string content, int type, string sender)
    {
        string url = $"{rootURL}/sms/send";
        if (phones.Length <= 0)
            return "";
        if (content.Equals(""))
            return "";
        if (type == TYPE_BRANDNAME && sender.Equals(""))
            return "";

        var builder = new
        {
            to = phones,
            content = Uri.EscapeDataString(content),
            type,
            sender
        };
        var response = await _client.PostAsJsonAsync(url, builder);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> SendMMSAsync(string[] phones, string content, string link, string sender)
    {
        string url = $"{rootURL}/mms/send";
        if (phones.Length <= 0)
            return "";
        if (content.Equals(""))
            return "";

        var builder = new
        {
            to = phones,
            content = Uri.EscapeDataString(content),
            link,
            sender
        };

        var response = await _client.PostAsJsonAsync(url, builder);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
