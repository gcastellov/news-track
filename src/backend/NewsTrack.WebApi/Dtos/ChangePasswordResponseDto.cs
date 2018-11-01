using NewsTrack.Identity.Results;

namespace NewsTrack.WebApi.Dtos
{
    public class ChangePasswordResponseDto : ResponseBaseDto
    {
        public enum FailureReason
        {
            PasswordsDontMatch = 1,
            InvalidCurrentPassword = 2
        }

        public FailureReason Failure { get; protected set; }

        private ChangePasswordResponseDto(ChangePasswordResult result)
        {
            IsSuccessful = result == ChangePasswordResult.Ok;
            if (!IsSuccessful)
            {
                Failure = (FailureReason) result;
            }
        }

        public static ChangePasswordResponseDto Create(ChangePasswordResult result)
        {
            return new ChangePasswordResponseDto(result);
        }
    }
}