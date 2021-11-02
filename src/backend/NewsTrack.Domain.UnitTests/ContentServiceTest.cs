using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NewsTrack.Domain.Entities;
using NewsTrack.Domain.Repositories;
using NewsTrack.Domain.Services;
using Xunit;
using FluentAssertions;

namespace NewsTrack.Domain.UnitTests
{
    
    public class ContentServiceTest
    {
        private Mock<IDraftRepository> _draftRepositoryMock;
        private Mock<IContentRepository> _contentRepositoryMock;
        private Mock<IDraftSuggestionsRepository> _draftSuggestionRepositoryMock;
        private Mock<IDraftRelationshipRepository> _draftRelationshipRepositoryMock;
        private ContentService _contentService;

        public ContentServiceTest()
        {
            _draftRepositoryMock = new Mock<IDraftRepository>();
            _contentRepositoryMock = new Mock<IContentRepository>();
            _draftSuggestionRepositoryMock = new Mock<IDraftSuggestionsRepository>();
            _draftRelationshipRepositoryMock = new Mock<IDraftRelationshipRepository>();
            _contentService = new ContentService(
                _draftRepositoryMock.Object, 
                _contentRepositoryMock.Object,
                _draftSuggestionRepositoryMock.Object,
                _draftRelationshipRepositoryMock.Object);
        }

        [Fact]
        public async Task WhenSettingSuggestions_ThenExtractTagsFromOtherContent()
        {
            var allTags = new[] {"Tag1", "Tag2", "Tag3", "Tag4"};
            var contentId1 = Guid.NewGuid();
            var contentId2 = Guid.NewGuid();
            var existingTags1 = new[] {"Tag2", "Tag3"};
            var existingTags2 = new[] {"Tag1"};

            var hightlights = new Dictionary<string, IEnumerable<string>>
            {
                { contentId1.ToString(), new [] { "something <em>Tag1</em>", "other <em>Tag4</em>" } },
                { contentId2.ToString(), new [] { "something <em>Tag1</em>", "other <em>Tag2</em>" } }
            };

            _draftRepositoryMock.Setup(m => m.GetTags())
                .Returns(Task.FromResult<IEnumerable<string>>(allTags));
            _contentRepositoryMock.Setup(m => m.GetHighlights(allTags))
                .Returns(Task.FromResult<IDictionary<string, IEnumerable<string>>>(hightlights));

            _draftRepositoryMock.Setup(m => m.GetTags(contentId1))
                .Returns(Task.FromResult<IEnumerable<string>>(existingTags1));
            _draftRepositoryMock.Setup(m => m.GetTags(contentId2))
                .Returns(Task.FromResult<IEnumerable<string>>(existingTags2));

            _draftSuggestionRepositoryMock.Setup(m => m.Save(It.IsAny<DraftSuggestions>())).Returns(Task.CompletedTask);

            await _contentService.SetSuggestions();

            _draftRepositoryMock.Verify(m => m.GetTags(contentId1), Times.Once);
            _draftRepositoryMock.Verify(m => m.GetTags(contentId2), Times.Once);
            _draftSuggestionRepositoryMock.Verify(m => m.Save(It.IsAny<DraftSuggestions>()), Times.Exactly(2));            
        }

        [Fact]        
        public void WhenHighlihtsFound_ThenExtractTagSuggestion()
        {
            var highlights = new[]
            {
                "<em>Francis</em> says that this is it",
                "what <em>Jason</em> says is not right",
                "<em>Jonas</em>",
                "this is what <em>Eve</em> told <em>Eric</em>"
            };

            var result = ContentService.ExtractTagSuggestion(highlights);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(new [] { "Francis", "Jason", "Jonas", "Eve", "Eric" });
        }

        [Fact]
        public void WhenHighlihtsFound_ThenGetTagSuggestion()
        {
            var contentId = Guid.NewGuid();
            var highlights = new[]
            {
                "<em>Francis</em> says that this is it",
                "what <em>Jason</em> says is not right",
                "<em>Jonas</em>",
                "this is what <em>Eve</em> told <em>Eric</em>"
            };

            var currentTags = new[] {"Francis", "Eve", "Jonas"};

            var result = ContentService.GetTagSuggestion(contentId, highlights, currentTags);

            result.Should().NotBeNull();
            result.Id.Should().Be(contentId);
            result.Tags.Should().BeEquivalentTo(new[] { "Jason", "Eric" });
        }
    }
}