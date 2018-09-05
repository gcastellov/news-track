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
        public string Username { get; set; }
        public FailureReason Failure { get; set; }
    }
}