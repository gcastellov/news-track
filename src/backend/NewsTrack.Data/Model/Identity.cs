using System;
using Nest;

namespace NewsTrack.Data.Model
{
    public class Identity : IDocument
    {
        public Guid Id { get; set; }
        [Keyword(Store = true)]
        public string Username { get; set; }
        [Keyword(Store = true)]
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public string Password { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public uint AccessFailedCount { get; set; }
        public string SecurityStamp { get; set; }
        public int UserType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}