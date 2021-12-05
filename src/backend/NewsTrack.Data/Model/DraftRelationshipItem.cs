using System;

namespace NewsTrack.Data.Model
{
    public record DraftRelationshipItem : IDocument
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }        
    }
}