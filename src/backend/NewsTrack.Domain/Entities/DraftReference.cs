using System;

namespace NewsTrack.Domain.Entities
{
    public class DraftReference
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}