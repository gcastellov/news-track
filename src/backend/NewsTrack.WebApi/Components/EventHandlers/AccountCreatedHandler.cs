using MediatR;
using NewsTrack.Common.Validations;
using NewsTrack.Identity.Events;
using NewsTrack.WebApi.Configuration;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NewsTrack.WebApi.Components.EventHandlers
{
    internal class AccountCreatedHandler : INotificationHandler<AccountCreated>
    {
        private readonly INotificator _notificator;
        private readonly IConfigurationProvider _configuration;

        public AccountCreatedHandler(INotificator notificator, IConfigurationProvider configuration)
        {
            _notificator = notificator;
            _configuration = configuration;
        }

        public Task Handle(AccountCreated notification, CancellationToken cancellationToken)
            => _notificator.SendEmail(
                "You account has been created",
                GetBody(notification),
                notification.Identity.Email);

        private string GetBody(AccountCreated notification)
        {
            var uBuilder = new UriBuilder(_configuration.ApiUrl);
            string callback = HttpUtility.UrlEncode($"{_configuration.SignInUrl}?confirmed=true&email={notification.Identity.Email}");
            uBuilder.Path += $"/api/identity/confirm/{notification.Identity.Email}/{notification.Identity.SecurityStamp}";
            uBuilder.Query = $"go={callback}";

            var sBuilder = new StringBuilder();
            sBuilder.Append("Your account has been created. Please confirm it by clicking the next link:");
            sBuilder.Append("<br/><br/>");
            sBuilder.Append($"<a href='{uBuilder.Uri.AbsoluteUri}'>Confirm your account</a>");

            if (notification.ClearPassword.HasValue())
            {
                sBuilder.Append("<br/><br/>");
                sBuilder.Append("A password has been provided, however, you can change it from your control panel at any time:");
                sBuilder.Append("<br/><br/>");
                sBuilder.Append("<b>");
                sBuilder.Append(notification.ClearPassword);
                sBuilder.Append("</b>");
            }

            return sBuilder.ToString();
        }
    }
}
