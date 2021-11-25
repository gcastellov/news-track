using MediatR;

namespace NewsTrack.Identity.Events
{
    public class AccountLocked : INotification
    {
        public Identity Identity { get; private set; }

        public static AccountLocked From(Identity identity)
        {
            return new AccountLocked { Identity = identity };
        }
    }
}
