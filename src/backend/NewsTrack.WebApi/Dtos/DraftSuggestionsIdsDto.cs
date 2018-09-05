using System;
using System.Collections.Generic;

namespace NewsTrack.WebApi.Dtos
{
    public class DraftSuggestionsIdsDto
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> SuggestedIds { get; set; }
        public int Count { get; set; }
    }
}
