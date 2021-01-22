using System;

namespace NewsTrack.Data.Model
{
    public class DraftRelationshipItem : IDocument
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }        
    }
}