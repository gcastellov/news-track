using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using NewsTrack.Domain.Entities;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class NewsSuggestionsEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/news/entry/{id}/suggestions";

        public NewsSuggestionsEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingNews_WhenGettingRelatedSuggestion_ThenReturnsCollection()
        {
            // Arrange
            var suggestedDraft = CreateDraftEntity();
            var draftSuggestions = new DraftSuggestions
            {
                Id = Guid.NewGuid(),
                Tags = new [] { "tag1", "tag2" },
                Drafts = new[] 
                { 
                    new Draft
                    {
                        Id = suggestedDraft.Id,
                        CreatedAt = suggestedDraft.CreatedAt,
                    }
                }
            };
            
            string urlRequest = Endpoint.Replace("{id}", draftSuggestions.Id.ToString());
            var endpoint = GetUriWithQueryString(urlRequest, new Tuple<string, object>("take", 10));

            Factory.DraftSuggestionsRepositoryMock.Setup(m => m.Get(draftSuggestions.Id)).Returns(Task.FromResult(draftSuggestions));
            Factory.DraftRepositoryMock.Setup(m => m.Get(suggestedDraft.Id)).Returns(Task.FromResult(suggestedDraft));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<DraftSuggestionsDto>();
            envelope.ShouldBeSuccessful();

            envelope.Payload.Id.Should().Be(draftSuggestions.Id);
            envelope.Payload.Tags.Should().BeEquivalentTo(draftSuggestions.Tags);
            envelope.Payload.Drafts.Should().HaveCount(draftSuggestions.Drafts.Count());
            
            var suggestion = envelope.Payload.Drafts.First();
            suggestion.Id.Should().Be(suggestedDraft.Id);
            suggestion.Title.Should().Be(suggestedDraft.Title);
            suggestion.Url.Should().Be(suggestedDraft.Uri);
        }

        [Fact]
        public async Task GivenInvalidRequestWithNonPositiveTake_WhenGettingRelatedSuggestion_ThenReturnBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid(); 
            string urlRequest = Endpoint.Replace("{id}", id.ToString());
            var endpoint = GetUriWithQueryString(urlRequest, new Tuple<string, object>("take", 0));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeBadRequest();
        }

        [Fact]
        public async Task GivenInvalidRequestWithoutTake_WhenGettingRelatedSuggestion_ThenReturnBadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            string endpoint = Endpoint.Replace("{id}", id.ToString());

            // Act
            var response = await Client.GetAsync(endpoint);

            // Assert
            response.ShouldBeBadRequest();
        }
    }
}
