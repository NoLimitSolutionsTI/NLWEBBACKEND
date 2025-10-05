namespace NLBackend.Models
{
    public class SmtpSettings
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string User { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string FromName { get; set; } = "No-Reply";
        public string FromAddress { get; set; } = default!;
        public string ToAddressOnCreate { get; set; } = default!;
    }
}
