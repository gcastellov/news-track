namespace NewsTrack.Common.Events
{
    public class NotificationEventArgs
    {
        public enum NotificationType
        {
            AccountLockout,
            AccountConfirmed,
            AccountCreated,
        }

        public string To { get; set; }
        public string Username { get; set; }
        public NotificationType Type { get; set; }
        public dynamic Model { get; set; }
    }
}