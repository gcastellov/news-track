namespace NewsTrack.Identity.Results
{
    public class SaveIdentityResult
    {
        public enum ResultType
        {
            Ok,
            InvalidEmailPattern,
            PasswordsDontMatch,
            InvalidUsername,
            InvalidEmail,
            ExistingAccount
        }

        public Identity Identity { get; }
        public ResultType Type { get; }

        private SaveIdentityResult(Identity identity, ResultType type)
        {
            Identity = identity;
            Type = type;
        }

        public static SaveIdentityResult As(ResultType type)
        {
            return new SaveIdentityResult(null, type);
        }

        public static SaveIdentityResult Create(Identity identity, ResultType type)
        {
            return new SaveIdentityResult(identity, type);
        }
    }
}