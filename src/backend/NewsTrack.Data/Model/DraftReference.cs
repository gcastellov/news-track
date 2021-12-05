using System;

namespace NewsTrack.Data.Model
{
    public record DraftReference : IDocument
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
