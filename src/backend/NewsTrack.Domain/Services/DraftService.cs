using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;
using NewsTrack.Common.Validations;

namespace NewsTrack.Domain.Services
{
    public class DraftService : IDraftService
    {
        private readonly IDraftRepository _draftRepository;
        private readonly IContentRepository _contentRepository;
        private readonly IDraftRelationshipRepository _draftRelationshipRepository;

        public DraftService(
            IDraftRepository draftRepository,
            IContentRepository contentRepository,
            IDraftRelationshipRepository draftRelationshipRepository)
        {
            _draftRepository = draftRepository;
            _contentRepository = contentRepository;
            _draftRelationshipRepository = draftRelationshipRepository;
        }

        public async Task Save(Draft draft, string body)
        {
            draft.Uri.Check(nameof(draft.Uri));
            draft.Picture.Check(nameof(draft.Title));
            draft.Paragraphs.CheckIfNull(nameof(draft.Paragraphs));
            draft.Title.CheckIfNull(nameof(draft.Title));
            draft.User.CheckIfNull(nameof(draft.User));
            body.CheckIfNull(nameof(body));

            var content = new Content
            {
                Id = draft.Id,
                Body = body
            };

            await _draftRepository.Save(draft);
            await _contentRepository.Save(content);
        }

        public async Task SetRelationships(Guid id, IEnumerable<DraftRelationshipItem> items)
        {
            var relatedDrafts = items?.ToList();
            relatedDrafts.CheckIfNull(nameof(items));
            relatedDrafts.ForEach(r =>
            {
                r.Id.CheckIfNull("Id");
                r.Title.CheckIfNull("Title");
                r.Url.Check("Url");
            });

            var relationship = new DraftRelationship
            {
                Id = id,
                Relationship = relatedDrafts
            };

            await _draftRelationshipRepository.SetRelationship(relationship);
            await _draftRepository.SetRelationship(id, relatedDrafts.Count);
        }
    }
}