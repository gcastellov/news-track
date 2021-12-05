using System;
using System.Collections.Generic;

namespace NewsTrack.Data.Model
{
    public record DraftRelationship : IDocument
    {
        public Guid Id { get; set; }
        public IEnumerable<DraftRelationshipItem> Relationship { get; set; }
    }
}