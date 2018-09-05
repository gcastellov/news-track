using System;
using System.Collections.Generic;

namespace NewsTrack.Domain.Entities
{
    public class DraftRelationship
    {
        public Guid Id { get; set; }
        public IEnumerable<DraftRelationshipItem> Relationship { get; set; }
    }
}