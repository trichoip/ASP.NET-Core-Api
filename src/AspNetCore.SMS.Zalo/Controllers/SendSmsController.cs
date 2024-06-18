using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.SMS.Zalo.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SendSmsController : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> Send(string phonenumber, string messages)
    {
        using HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("secret_key", "bjh58jXJFntL2t3K1700");

        var url = "https://oauth.zaloapp.com/v4/oa/access_token";

        var data = new
        {
            app_id = "3825182632958705872",
            grant_type = "refresh_token",
            refresh_token = "8P5nImat4neXadD10ZGO7oYISq40Bt9WIjziIc8JVMuLpMnbVJLdQoljOJ1NAc8U1Cb762utKmDIq4DJ0ZGf3ccMDL09F1D_9-uJTMb5S2iXfdeoKd5x4NsGRYXLRdaB5vLeBcysGGKYbreaSsTTUGR3Q5fK57fG7RDjGLLfT1WVodSeT393A0YNAHPMO4ms2fvnFYLhTX0Fas4xN5PX1IpeO2077qSmNirPQ2OHHMuAiMOrLrCDRGBs3IbW9mqnDS8E7nOE9nfwmZuT8oDR07VwRmqfMGi5LeqGEZHL4p9YaGuf3dOh1NcnAoiFU3St8fySBMbgV2KmWri_NLCwUnwwK2PqN4er8PrX3LuzUm48vbmBPHPYB2UoQI5PMt1nReCIJpvw3dbAYWGb9abk5bgFU4u9IK5zSKSQarDf3omH6W"
        };

        var response = await httpClient.PostAsJsonAsync(url, data);
        var jsonString = await response.Content.ReadAsStringAsync();

        return Ok(jsonString);

    }
}
