using System;

namespace NewsTrack.Identity
{
    public class Identity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public string Password { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public uint AccessFailedCount { get; set; }
        public string SecurityStamp { get; set; }
        public IdentityTypes IdType { get; set; }
        public DateTime CreatedAt { get; set; }

        public Identity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}