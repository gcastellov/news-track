using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NewsTrack.WebApi.Configuration;

namespace NewsTrack.WebApi.Components
{
    internal class Notificator : INotificator
    {
        private readonly SmtpConfiguration _smtpConfiguration;
        private readonly ILogger<Notificator> _logger;

        public Notificator(IConfigurationProvider configurationProvider, ILogger<Notificator> logger)
        {
            _smtpConfiguration = configurationProvider.SmtpConfiguration;
            _logger = logger;
        }

        public async Task SendEmail(string subject, string content, string recipient)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_smtpConfiguration.From ?? "sender@news-track.com"),
                Subject = subject,
                Body = content,
                IsBodyHtml = true,
            };

            message.To.Add(new MailAddress(recipient));

            using (var emailSender = GetClient())
            await emailSender.SendMailAsync(message);

            _logger.LogDebug($"An email with subject '{subject}' has been sent");
        }

        private SmtpClient GetClient()
        {
            if (_smtpConfiguration.IsSet)
            {
                return new SmtpClient(_smtpConfiguration.Host, _smtpConfiguration.Port)
                {
                    Credentials = new NetworkCredential(_smtpConfiguration.Username, _smtpConfiguration.Password)
                };
            }

            var outbox = Path.Combine(GetExecutingDirectoryName(), "outbox");
            if (!Directory.Exists(outbox))
            {
                Directory.CreateDirectory(outbox);
            }

            return new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = outbox,
                Host = "localhost",
                UseDefaultCredentials = true,
                EnableSsl = false
            };
        }

        private static string GetExecutingDirectoryName()
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            return new FileInfo(location.AbsolutePath).Directory.FullName;
        }
    }
}