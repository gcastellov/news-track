using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;

namespace NewsTrack.Domain.Services
{
    public class ContentService : IContentService
    {
        private readonly IDraftRepository _draftRepository;
        private readonly IContentRepository _contentRepository;
        private readonly IDraftSuggestionsRepository _draftSuggestionsRepository;
        private readonly IDraftRelationshipRepository _draftRelationshipRepository;

        public ContentService(
            IDraftRepository draftRepository, 
            IContentRepository contentRepository, 
            IDraftSuggestionsRepository draftSuggestionsRepository, 
            IDraftRelationshipRepository draftRelationshipRepository)
        {
            _draftRepository = draftRepository;
            _contentRepository = contentRepository;
            _draftSuggestionsRepository = draftSuggestionsRepository;
            _draftRelationshipRepository = draftRelationshipRepository;
        }

        public async Task SetSuggestions()
        {
            var allTags = await _draftRepository.GetTags();
            var highlightsByContent = await _contentRepository.GetHighlights(allTags);

            foreach (var highlights in highlightsByContent)
            {
                var contentId = Guid.Parse(highlights.Key);
                var tags = await _draftRepository.GetTags(contentId);
                var draftSuggestions = GetTagSuggestion(contentId, highlights.Value, tags);
                if (draftSuggestions.Tags.Any())
                {
                    var entrySuggestions = await _draftRepository.Get(draftSuggestions.Tags);
                    var entryRelationship = await _draftRelationshipRepository.Get(contentId);
                    draftSuggestions.SetDraftSuggestions(entrySuggestions, entryRelationship);
                }

                await _draftSuggestionsRepository.Save(draftSuggestions);
            }
        }

        public static DraftSuggestions GetTagSuggestion(
            Guid contentId,
            IEnumerable<string> highlights, 
            IEnumerable<string> existingTags)
        {
            var matchingTags = ExtractTagSuggestion(highlights);
            var draftSuggestion = new DraftSuggestions(existingTags) { Id = contentId };
            draftSuggestion.SetTagSuggestions(matchingTags);
            return draftSuggestion;
        }

        public static HashSet<string> ExtractTagSuggestion(IEnumerable<string> highlights)
        {
            const string pattern = @"<em>(.*?)</em>";

            HashSet<string> result = new HashSet<string>();
            var regex = new Regex(pattern, RegexOptions.Multiline);

            foreach (var highlight in highlights)
            {
                var matching = regex.Matches(highlight);
                foreach (Match match in matching)
                {
                    result.Add(match.Groups[1].Value);
                }
            }

            return result;
        }
    }
}