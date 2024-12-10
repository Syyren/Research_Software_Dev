using Azure.Identity;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Research_Software_Dev.Services.EmailService;
using Azure.Core;
using System.Net.Http.Headers;
using Microsoft.Graph.Users.Item.SendMail;
using System.Net.Mail;
using System.Net;

public class Email : IEmail
{
    //injecting emailsettings and azure settings
    private readonly EmailServiceSettings _emailSettings;
    private readonly AzureServiceSettings _azureSettings;
    public Email(IOptions<AzureServiceSettings> azureSettings, IOptions<EmailServiceSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
        _azureSettings = azureSettings.Value;
    }

    //creating emailsender service as task
    public Task SendEmailAsync(string receiverEmail, string subject, string message)
    {
        try
        {
            var client = new SmtpClient(_emailSettings.SmtpHost, _emailSettings.SmtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(receiverEmail);

            return client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            throw new Exception("Email sending failed", ex);
        }
    }

}
