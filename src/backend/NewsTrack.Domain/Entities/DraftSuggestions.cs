using System;
using System.Collections.Generic;
using System.Linq;

namespace NewsTrack.Domain.Entities
{
    public class DraftSuggestions
    {
        private readonly IEnumerable<string> _currentTags;

        public DraftSuggestions() { }

        public DraftSuggestions(IEnumerable<string> currentTags)
        {
            _currentTags = currentTags;
        }

        public Guid Id { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<Draft> Drafts { get; set; }

        public void SetTagSuggestions(IEnumerable<string> allSuggestions)
        {
            Tags = _currentTags != null 
                ? allSuggestions.Except(_currentTags, StringComparer.CurrentCultureIgnoreCase)
                    .Select(s => s[0].ToString().ToUpper() + s.Substring(1).ToLowerInvariant())
                    .ToArray()
                : allSuggestions.ToArray();
        }

        public void SetDraftSuggestions(IEnumerable<Draft> allSuggestions, DraftRelationship relationship)
        {
            var ids = new List<Guid> {Id};
            if (relationship?.Relationship != null)
            {
                ids.AddRange(relationship.Relationship.Select(i => i.Id));
            }
            Drafts = allSuggestions.Where(d => !ids.Contains(d.Id));
        }
    }
}