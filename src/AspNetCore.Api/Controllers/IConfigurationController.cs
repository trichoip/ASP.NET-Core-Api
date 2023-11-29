using AspNetCore.Api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspNetCore.Api.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class IConfigurationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly MailSettings _mailSettings;
    private readonly JWTSettings _jwtSettings;
    private readonly PositionOptions _options;

    public IConfigurationController(
        IConfiguration configuration,
        IOptions<MailSettings> mailSettings,
        JWTSettings jwtSettings,
        IOptions<PositionOptions> options)
    {
        _configuration = configuration;
        _jwtSettings = jwtSettings;
        _mailSettings = mailSettings.Value;
        _options = options.Value;

    }

    [HttpGet]
    public IActionResult Ctor()
    {
        return Ok(new { _mailSettings, _jwtSettings, _options });
    }

    [HttpGet]
    public IActionResult Tree()
    {
        var isInMemory1 = _configuration["UseInMemoryDatabase"];
        var isInMemory2 = _configuration["UseInMemoryDatabase"] == "False";
        var key = _configuration["JWTSettings:Key"];
        var log = _configuration["Logging:LogLevel:Default"];

        return Ok(new
        {
            isInMemory1,
            isInMemory2,
            key,
            log,

        });
    }

    [HttpGet]
    public IActionResult Func()
    {
        // get param
        var isInMemory = _configuration.GetValue("UseInMemoryDatabase", true); // nếu không có thì là true

        // get param in object
        var key = _configuration.GetSection("JWTSettings").GetValue<string>("Key");
        var log2 = _configuration.GetSection("Logging:LogLevel").GetValue<string>("Default");

        var key2 = _configuration.GetValue<string>("JWTSettings:Key");
        var log = _configuration.GetValue<string>("Logging:LogLevel:Default");

        // get object
        var JWTSettings1 = _configuration.GetSection("JWTSettings").Get<JWTSettings>();
        var log3 = _configuration.GetSection("Logging").GetSection("LogLevel").Get<IDictionary<string, object>>();

        var positionOptions = new PositionOptions();
        _configuration.GetSection(PositionOptions.Position).Bind(positionOptions);

        var positionOptions2 = _configuration.GetSection(PositionOptions.Position).Get<PositionOptions>();

        var check = _configuration.GetSection("Logging").Exists();
        var child = _configuration.GetSection("Logging").GetChildren();

        return Ok(new
        {
            isInMemory,
            key,
            key2,
            JWTSettings1,
            positionOptions,
            positionOptions2,
            log,
            log2,
            log3,
            check,
            child,
        });
    }
}

public class PositionOptions
{
    public const string Position = "Position";

    public string Title { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;
}