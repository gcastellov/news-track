using MediatR;

namespace NewsTrack.Identity.Events
{
    public class AccountCreated : INotification
    {
        public Identity Identity { get; private set; }
        public string ClearPassword { get; private set; }

        public static AccountCreated From(Identity identity, string clearPassword)
        {
            return new AccountCreated
            {
                Identity = identity,
                ClearPassword = clearPassword
            };
        }

        public static AccountCreated From(Identity identity)
        {
            return new AccountCreated
            {
                Identity = identity
            };
        }
    }
}
