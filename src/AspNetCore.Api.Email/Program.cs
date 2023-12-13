using AspNetCore.Api.Email.Email;
using System.Net;
using System.Net.Mail;

namespace AspNetCore.Api.Email;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // config email
        var smtpClient = new SmtpClient
        {
            Host = builder.Configuration["Smtp:Host"],
            Port = int.Parse(builder.Configuration["Smtp:Port"]),
            Credentials = new NetworkCredential(builder.Configuration["Smtp:Username"], builder.Configuration["Smtp:Password"]),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = true,
        };

        builder.Services.AddSingleton(smtpClient);
        builder.Services.AddTransient<IEmailSender, EmailSender>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}