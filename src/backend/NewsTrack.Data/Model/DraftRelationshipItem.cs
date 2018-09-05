using System;

namespace NewsTrack.Data.Model
{
    public class DraftRelationshipItem : IDocument
    {
        public Guid Id { get; set; }
        public Uri Url { get; set; }
        public string Title { get; set; }        
    }
}