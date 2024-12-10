using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Research_Software_Dev.Services.EmailService
{
    public class Email : IEmail
    {
        //injecting emailsettings
        private readonly EmailSettings _emailSettings;
        public Email(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
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
}
