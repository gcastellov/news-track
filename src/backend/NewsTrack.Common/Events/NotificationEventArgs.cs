namespace NewsTrack.Common.Events
{
    public class NotificationEventArgs
    {
        public enum NotificationType
        {
            AccountLockout,
            AccountConfirmed
        }

        public string To { get; set; }
        public string Username { get; set; }
        public NotificationType Type { get; set; }
    }
}