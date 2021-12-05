using System;

namespace NewsTrack.Data.Model
{
    public record Website : IDocument
    {
        public Guid Id { get; set; }
        
        public string Uri { get; set; }
    }
}