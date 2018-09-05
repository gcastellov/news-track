using System;

namespace NewsTrack.Data.Model
{
    public class DraftReference : IDocument
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
