using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class SearchEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/search";

        public SearchEndpointTests(TestWebAppFactory<Startup> testWebAppFactory)
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingDrafts_WhenSearching_ThenReturnMatchings()
        {
            // Arrange
            const string query = "some mathing pattern";
            var endpoint = GetUriWithQueryString(Endpoint, new Tuple<string, object>("query", query));
            var draftResult = CreateDraftEntity();

            var results = new[] { draftResult };

            Factory.DraftRepositoryMock.Setup(m => m.Search(query)).Returns(Task.FromResult(results.AsEnumerable()));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<IEnumerable<SearchResultDto>>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.Should().HaveCount(results.Length);
            
            var dto = envelope.Payload.First();
            dto.Id.Should().Be(draftResult.Id);
            dto.Content.Should().Be(draftResult.Title);
        }
    }
}
