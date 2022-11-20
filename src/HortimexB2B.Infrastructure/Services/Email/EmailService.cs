using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HortimexB2B.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public void Send(string receiver, string subject, string messageContent)
        {
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Connect(_settings.ServerAddress, _settings.Port, SecureSocketOptions.Auto);

                client.Authenticate(_settings.Username, _settings.Password);

                var message = new MimeMessage();
                message.To.Add(new MailboxAddress(receiver));
                message.From.Add(new MailboxAddress("Hortimex B2B", "b2b@hortimex.pl"));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = messageContent;

                message.Body = bodyBuilder.ToMessageBody();

                client.Send(message);

                client.Disconnect(true);
            }
        }
    }
}