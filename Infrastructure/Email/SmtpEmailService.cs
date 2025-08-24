using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
namespace Infrastructure.Email;

public class SmtpEmailService(EmailTemplateBuilder emailTemplateBuilder, IOptions<EmailConfig> _emailConfig, IConfiguration configuration) : Application.Services.IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailConfig.Value.SenderName, _emailConfig.Value.SenderEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };
        message.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_emailConfig.Value.SmtpServer, _emailConfig.Value.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailConfig.Value.SenderEmail, _emailConfig.Value.EmailPassword);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task SendAccountCreatedEmail(string userEmail, string userName, string password)
    {
     var baseUrl = configuration["BaseUrl"];
     var loginUrl = $"{baseUrl}/Account/Login";
    var body = emailTemplateBuilder.BuildEmailBody(Application.Services.IEmailService.EmailTemplatePaths.AccountCreated,
             new Dictionary<string, string>
             {
                { "UserName", userName },
                { "UserEmail", userEmail },
                 {"UserPassword", password },
                 {"LoginURL", loginUrl},
             }
           );

        await SendEmailAsync(userEmail, "Account Created", body);
    }
}