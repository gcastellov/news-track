namespace NewsTrack.WebApi.Dtos
{
    public class ChangePasswordResponseDto : ResponseBaseDto
    {
        public enum FailureReason
        {
            PasswordsDontMatch = 1,
            InvalidCurrentPassword = 2
        }

        public FailureReason Failure { get; set; }
    }
}