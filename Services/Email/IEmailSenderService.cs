namespace PersonalAccount.Services.Email;

public interface IEmailSenderService
{
    Task SendEmailAsync(string to, string subject, string html);
}