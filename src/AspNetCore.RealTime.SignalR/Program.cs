using AspNetCore.RealTime.SignalR.Hubs;
using MessagePack;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;

    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10);
    hubOptions.MaximumReceiveMessageSize = 65_536;
    hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(15);
    hubOptions.MaximumParallelInvocationsPerClient = 2;
    hubOptions.StreamBufferCapacity = 15;

    if (hubOptions?.SupportedProtocols is not null)
    {
        foreach (var protocol in hubOptions.SupportedProtocols)
            Console.WriteLine($"SignalR supports {protocol} protocol.");
    }

})
.AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
    //options.PayloadSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.PayloadSerializerOptions.Encoder = null;
    options.PayloadSerializerOptions.IncludeFields = false;
    options.PayloadSerializerOptions.IgnoreReadOnlyFields = false;
    options.PayloadSerializerOptions.IgnoreReadOnlyProperties = false;
    options.PayloadSerializerOptions.MaxDepth = 0;
    options.PayloadSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
    options.PayloadSerializerOptions.DictionaryKeyPolicy = null;
    options.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    options.PayloadSerializerOptions.PropertyNameCaseInsensitive = false;
    options.PayloadSerializerOptions.DefaultBufferSize = 32_768;
    options.PayloadSerializerOptions.ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip;
    options.PayloadSerializerOptions.ReferenceHandler = null;
    options.PayloadSerializerOptions.UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement;
    options.PayloadSerializerOptions.WriteIndented = true;

    Console.WriteLine($"Number of default JSON converters: {options.PayloadSerializerOptions.Converters.Count}");
})
.AddMessagePackProtocol(options =>
{
    options.SerializerOptions = MessagePackSerializerOptions.Standard
        .WithSecurity(MessagePackSecurity.UntrustedData)
        .WithCompression(MessagePackCompression.Lz4Block)
        .WithAllowAssemblyVersionMismatch(true)
        .WithOldSpec()
        .WithOmitAssemblyVersion(true);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PresenceTracker>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("demokeydemokeydemokeydemokeydemokeydemokey")),
                       ValidateIssuerSigningKey = true,
                       ClockSkew = TimeSpan.Zero,
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       NameClaimType = ClaimTypes.NameIdentifier
                   };
                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var accessToken = context.Request.Query["access_token"];
                           if (!string.IsNullOrEmpty(accessToken))
                           {
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };
               });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.MapHub<SignalrServer>("/signalrServer", options =>
{
    options.Transports =
                HttpTransportType.WebSockets |
                HttpTransportType.LongPolling;
    options.CloseOnAuthenticationExpiration = true;
    options.ApplicationMaxBufferSize = 65_536;
    options.TransportMaxBufferSize = 65_536;
    options.MinimumProtocolVersion = 0;
    options.TransportSendTimeout = TimeSpan.FromSeconds(10);
    options.WebSockets.CloseTimeout = TimeSpan.FromSeconds(3);
    options.LongPolling.PollTimeout = TimeSpan.FromSeconds(10);

    Console.WriteLine($"Authorization data items: {options.AuthorizationData.Count}");
});

app.MapHub<ChatHub>("/chatHub");
app.MapHub<OfferHub>("/offers");
app.MapHub<ChatHubV2>("/chatHubV2");

app.Run();
