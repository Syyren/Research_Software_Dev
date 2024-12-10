namespace Research_Software_Dev.Services.EmailService
{
    public interface IEmail
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
