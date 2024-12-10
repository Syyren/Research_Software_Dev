namespace Research_Software_Dev.Services.EmailService
{
    public class EmailServiceSettings
    {
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderPassword { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
    }

    public class AzureServiceSettings
    {
        public string ClientId { get; set; }
        public string TenantId { get; set; }
        public string ClientSecret { get; set; }
    }
}
