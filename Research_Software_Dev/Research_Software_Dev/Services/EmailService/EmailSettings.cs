namespace Research_Software_Dev.Services.EmailService
{
    public class EmailSettings
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
    }
}
