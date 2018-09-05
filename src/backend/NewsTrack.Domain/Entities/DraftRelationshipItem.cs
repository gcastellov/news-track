using System;

namespace NewsTrack.Domain.Entities
{
    public class DraftRelationshipItem
    {
        public Guid Id { get; set; }
        public Uri Url { get; set; }
        public string Title { get; set; }        
    }
}