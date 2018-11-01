using NewsTrack.Identity.Results;

namespace NewsTrack.WebApi.Dtos
{
    public class TokenResponseDto : ResponseBaseDto
    {
        public enum FailureReason
        {
            Authentication = 1,
            Lockout = 2
        }

        public string Token { get; set; }
        public string Username { get; protected set; }
        public FailureReason Failure { get; protected set; }

        private TokenResponseDto(AuthenticateResult result, string username)
        {
            IsSuccessful = result == AuthenticateResult.Ok;
            Username = username;
            if (!IsSuccessful)
            {
                Failure = (FailureReason)result;
            }
        }

        public static TokenResponseDto Create(AuthenticateResult result, string username)
        {
            return new TokenResponseDto(result, username);
        }
    }
}