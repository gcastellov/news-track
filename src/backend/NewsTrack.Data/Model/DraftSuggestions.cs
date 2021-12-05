using System;
using System.Collections.Generic;

namespace NewsTrack.Data.Model
{
    public record DraftSuggestions : IDocument
    {
        public Guid Id { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<DraftReference> Drafts { get; set; }
    }
}