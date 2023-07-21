using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using TimeOffManagementAPI.Business.Interfaces;

namespace TimeOffManagementAPI.Business.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmaiAsync(string recipient, string subject, string body) // TODO html tasarım ayarla
    {
        string? fromAddress = _configuration["EmailSettings:FromAddress"];
        string? smtpServer = _configuration["EmailSettings:SmtpServer"];
        string? smtpPortStr = _configuration["EmailSettings:SmtpPort"];
        string? username = _configuration["EmailSettings:Username"];
        string? password = _configuration["EmailSettings:Password"];
        string? enableSslStr = _configuration["EmailSettings:EnableSsl"];

        if (string.IsNullOrEmpty(fromAddress) || string.IsNullOrEmpty(smtpServer) ||
            string.IsNullOrEmpty(smtpPortStr) || string.IsNullOrEmpty(username) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(enableSslStr))
        {
            throw new ApplicationException("SMTP configuration is missing or invalid.");
        }

        if (!int.TryParse(smtpPortStr, out int smtpPort))
        {
            throw new ApplicationException("SMTP port configuration is invalid.");
        }

        if (!bool.TryParse(enableSslStr, out bool enableSsl))
        {
            throw new ApplicationException("SMTP SSL configuration is invalid.");
        }

        using (var client = new SmtpClient(smtpServer, smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = enableSsl;

            var message = new MailMessage(fromAddress, recipient, subject, body);
            message.IsBodyHtml = true;

            await client.SendMailAsync(message);
        }
    }
}

