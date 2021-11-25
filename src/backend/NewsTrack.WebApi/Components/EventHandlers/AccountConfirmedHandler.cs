using MediatR;
using NewsTrack.Identity.Events;
using System.Threading;
using System.Threading.Tasks;

namespace NewsTrack.WebApi.Components.EventHandlers
{
    internal class AccountConfirmedHandler : INotificationHandler<AccountConfirmed>
    {
        private readonly INotificator _notificator;

        public AccountConfirmedHandler(INotificator notificator)
        {
            _notificator = notificator;
        }

        public Task Handle(AccountConfirmed notification, CancellationToken cancellationToken)
            => _notificator.SendEmail(
                "Your account has been confirmed",
                "Your account is enabled and ready to be used.",
                notification.Identity.Email);
    }
}
