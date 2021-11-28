namespace NewsTrack.Identity.Results
{
    public enum ChangePasswordResult : uint
    {
        Ok = 0,
        PasswordsDontMatch = 1,
        InvalidCurrentPassword = 2
    }
}