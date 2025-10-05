using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NLBackend.Models;

namespace NLBackend.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpSettings _cfg;

        public SmtpEmailSender(IOptions<SmtpSettings> cfg)
        {
            _cfg = cfg.Value;
        }

        public async Task SendAsync(string to, string subject, string htmlBody, string? bcc = null)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_cfg.FromName, _cfg.FromAddress));
            msg.To.Add(MailboxAddress.Parse(to));
            if (!string.IsNullOrWhiteSpace(bcc))
                msg.Bcc.Add(MailboxAddress.Parse(bcc));
            msg.Subject = subject;
            msg.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var client = new SmtpClient();
            var socketOpt = _cfg.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
            await client.ConnectAsync(_cfg.Host, _cfg.Port, socketOpt);
            if (!string.IsNullOrWhiteSpace(_cfg.User)) 
                await client.AuthenticateAsync(_cfg.User, _cfg.Password);

            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
    }
}
