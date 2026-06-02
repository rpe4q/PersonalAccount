using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace PersonalAccount.Services.Email;

public class EmailSender(IOptions<SmtpSettings> options) : IEmailSender
{
    private readonly SmtpSettings _settings = options.Value;

    public async Task SendEmailAsync(string to, string subject, string html)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromEmail, _settings.FromName));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new BodyBuilder
        {
            HtmlBody = html,
        }.ToMessageBody();

        using var client = new SmtpClient();
        client.Timeout = _settings.Timeout;
        await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_settings.Username, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}