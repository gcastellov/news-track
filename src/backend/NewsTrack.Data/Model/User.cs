using System;

namespace NewsTrack.Data.Model
{
    public record User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
    }
}