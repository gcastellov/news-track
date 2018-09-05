using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NewsTrack.Common.Events;
using NewsTrack.WebApi.Configuration;

namespace NewsTrack.WebApi.Components
{
    public class NotificationManager
    {
        private readonly SmtpConfiguration _configuration;

        public NotificationManager(IConfigurationProvider configurationProvider)
        {
            _configuration = configurationProvider.SmtpConfiguration;
        }

        public void Handle(object sender, NotificationEventArgs args)
        {
            var message = new MailMessage
            {
                From = new MailAddress(_configuration.From),
                Subject = GetSubject(args.Type),
                Body = GetBody(args.Type),
                IsBodyHtml = true
            };

            message.To.Add(args.To);
            
            Task.Run(() => SendEmail(message));
        }

        private void SendEmail(MailMessage message)
        {
            using (var emailSender = new SmtpClient(_configuration.Host, _configuration.Port))
            {                
                emailSender.Credentials = new NetworkCredential(_configuration.Username, _configuration.Password);
                emailSender.Send(message);
            }
        }

        private string GetSubject(NotificationEventArgs.NotificationType type)
        {
            switch (type)
            {
                case NotificationEventArgs.NotificationType.AccountLockout:
                    return "Your account has been lock out";
                default:
                    throw new NotImplementedException();
            }
        }

        private string GetBody(NotificationEventArgs.NotificationType type)
        {
            var sBuilder = new StringBuilder();
            sBuilder.Append("<html>");
            sBuilder.Append("<p>");

            switch (type)
            {
                case NotificationEventArgs.NotificationType.AccountLockout:
                    sBuilder.Append("Your account has been locked out for security reasons");
                    break;
                default:
                    throw new NotImplementedException();
            }

            sBuilder.Append("</p>");
            sBuilder.Append("</html>");
            return sBuilder.ToString();
        }
    }
}