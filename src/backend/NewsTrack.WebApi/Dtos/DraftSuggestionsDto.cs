using System;
using System.Collections.Generic;

namespace NewsTrack.WebApi.Dtos
{
    public class DraftSuggestionsDto
    {
        public Guid Id { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<NewsDigestBaseDto> Drafts { get; set; }
    }
}