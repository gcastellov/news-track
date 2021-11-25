using MediatR;
using NewsTrack.Identity.Events;
using System.Threading;
using System.Threading.Tasks;

namespace NewsTrack.WebApi.Components.EventHandlers
{
    internal class AccountLockedHandler : INotificationHandler<AccountLocked>
    {
        private readonly INotificator _notificator;

        public AccountLockedHandler(INotificator notificator)
        {
            _notificator = notificator;
        }

        public Task Handle(AccountLocked notification, CancellationToken cancellationToken)
            => _notificator.SendEmail(
                "Your account has been lock out",
                "Your account has been locked out for security reasons.",
                notification.Identity.Email);
    }
}
