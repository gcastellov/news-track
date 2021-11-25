using MediatR;

namespace NewsTrack.Identity.Events
{
    public class AccountConfirmed : INotification
    {
        public Identity Identity { get; private set; }

        public static AccountConfirmed From(Identity identity)
        {
            return new AccountConfirmed { Identity = identity };
        }
    }
}
