namespace NewsTrack.Identity.Results
{
    public class SaveIdentityResult
    {
        public enum ResultType
        {
            Ok = 0,
            InvalidEmailPattern = 1,
            PasswordsDontMatch = 2,
            ExistingUsername = 3,
            ExistingEmail = 4
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