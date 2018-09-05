using System;
using NewsTrack.Identity;

namespace NewsTrack.WebApi.Dtos
{
    public class IdentityDto : ResponseBaseDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public uint AccessFailedCount { get; set; }
        public IdentityTypes IdType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}