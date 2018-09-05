using System;
using System.Collections.Generic;

namespace NewsTrack.Data.Model
{
    public class DraftRelationship : IDocument
    {
        public Guid Id { get; set; }
        public IEnumerable<DraftRelationshipItem> Relationship { get; set; }
    }
}