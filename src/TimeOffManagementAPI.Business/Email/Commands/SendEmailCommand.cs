using MediatR;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace TimeOffManagementAPI.Business.Email.Commands;

public record SendEmailCommand : IRequest
{
    public SendEmailCommand(string recipient, string subject, string body)
    {
        Recipient = recipient;
        Subject = subject;
        Body = body;
    }

    public string? Recipient { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
};

public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand>
{
    private readonly IConfiguration _configuration;

    public SendEmailCommandHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Handle(SendEmailCommand sendEmailCommand, CancellationToken cancellationToken)
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

        if (string.IsNullOrWhiteSpace(sendEmailCommand.Recipient))
            throw new ArgumentNullException("Recipient cannot be null.");

        using SmtpClient client = new(smtpServer, smtpPort);

        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(username, password);
        client.EnableSsl = enableSsl;

        MailMessage message = new(fromAddress, sendEmailCommand.Recipient, sendEmailCommand.Subject, sendEmailCommand.Body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message, cancellationToken);
    }
}