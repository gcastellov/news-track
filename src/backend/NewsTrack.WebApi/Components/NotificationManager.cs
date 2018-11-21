using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NewsTrack.Common.Events;
using NewsTrack.Common.Validations;
using NewsTrack.WebApi.Configuration;

namespace NewsTrack.WebApi.Components
{
    public class NotificationManager
    {
        private readonly SmtpConfiguration _smtpConfiguration;
        private readonly IConfigurationProvider _configuration;

        public NotificationManager(IConfigurationProvider configurationProvider)
        {
            _configuration = configurationProvider;
            _smtpConfiguration = configurationProvider.SmtpConfiguration;
        }

        public void Handle(object sender, NotificationEventArgs args)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_smtpConfiguration.From ?? "sender@news-track.com"),
                Subject = GetSubject(args.Type),
                Body = GetBody(args.Type, args.Model),
                IsBodyHtml = true
            };

            message.To.Add(args.To);

            Task.Run(() => SendEmail(message));
        }

        private void SendEmail(MailMessage message)
        {
            using (var emailSender = GetClient())
            {
                emailSender.Send(message);
            }
        }

        private string GetSubject(NotificationEventArgs.NotificationType type)
        {
            switch (type)
            {
                case NotificationEventArgs.NotificationType.AccountLockout:
                    return "Your account has been lock out";
                case NotificationEventArgs.NotificationType.AccountCreated:
                    return "You account has been created";
                case NotificationEventArgs.NotificationType.AccountConfirmed:
                    return "Your account has been confirmed";
                default:
                    throw new NotImplementedException();
            }
        }

        private string GetBody(NotificationEventArgs.NotificationType type, dynamic model)
        {
            var sBuilder = new StringBuilder();
            sBuilder.Append("<html>");
            sBuilder.Append("<p>");

            switch (type)
            {
                case NotificationEventArgs.NotificationType.AccountLockout:
                    sBuilder.Append("Your account has been locked out for security reasons");
                    break;
                case NotificationEventArgs.NotificationType.AccountCreated:
                    var uBuilder = new UriBuilder(_configuration.ApiUrl);
                    uBuilder.Path += $"/api/identity/confirm/{model.Email}/{model.SecurityStamp}";
                    uBuilder.Query = "go=" + _configuration.SignInUrl;
                    sBuilder.Append("Your account has been created. Please confirm it by clicking the next link:");
                    sBuilder.Append("<br/><br/>");
                    sBuilder.Append($"<a href='{uBuilder.Uri.AbsoluteUri}'>Confirm your account</a>");
                    if (((object)model).HasProperty("Password"))
                    {
                        sBuilder.Append("<br/><br/>");
                        sBuilder.Append("A password has been provided, however, you can change it at any time:");
                        sBuilder.Append("<br/><br/>");
                        sBuilder.Append("<b>");
                        sBuilder.Append(model.Password);
                        sBuilder.Append("</b>");
                    }
                    break;
                case NotificationEventArgs.NotificationType.AccountConfirmed:
                    sBuilder.Append("Your account is enabled and ready to be used.");
                    break;
                default:
                    throw new NotImplementedException();
            }

            sBuilder.Append("</p>");
            sBuilder.Append("</html>");
            return sBuilder.ToString();
        }

        private SmtpClient GetClient()
        {
            if (_configuration.SmtpConfiguration.IsSet)
            {
                return new SmtpClient(_smtpConfiguration.Host, _smtpConfiguration.Port)
                {
                    Credentials = new NetworkCredential(_smtpConfiguration.Username, _smtpConfiguration.Password)
                };
            }

            var outbox = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "outbox");
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
    }
}