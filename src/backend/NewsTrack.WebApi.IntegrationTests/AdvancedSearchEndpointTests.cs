using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class AdvancedSearchEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/search/advanced";
        private const string Query = "some mathing pattern";

        public AdvancedSearchEndpointTests(TestWebAppFactory<Program> testWebAppFactory)
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenInvalidRequestWithoutCount_WhenSearching_ThenReturnsBadRequest()
        {
            // Arrange
            var endpoint = GetUriWithQueryString(
                Endpoint,
                ("query", Query));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeBadRequest();
        }

        [Fact]
        public async Task GivenInvalidRequestWithNotPositiveCount_WhenSearching_ThenReturnsBadRequest()
        {
            // Arrange
            var endpoint = GetUriWithQueryString(
                Endpoint,
                ("query", Query),
                ("count", 0));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeBadRequest();
        }

        [Fact]
        public async Task GivenExistingDrafts_WhenSearchingUsingQueryAndCount_ThenReturnMatchings()
        {
            // Arrange
            const int count = 1;
            const long totalMatches = 1;

            var draftResult = CreateDraftEntity();
            var endpoint = GetUriWithQueryString(
                Endpoint,
                ("query", Query),
                ("count", count));

            var results = new[] { draftResult };

            Factory.DraftRepositoryMock.Setup(m => m.Get(null, Query, Array.Empty<string>(), 0, count)).Returns(Task.FromResult(results.AsEnumerable()));
            Factory.DraftRepositoryMock.Setup(m => m.Count(null, Query, Array.Empty<string>())).Returns(Task.FromResult(totalMatches));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeSuccessful();

            var envelope = await response.ShouldBeOfType<NewsResponseListDto>();
            envelope.Payload.Count.Should().Be(totalMatches);
            envelope.Payload.News.Should().HaveCount(results.Length);
            var dto = envelope.Payload.News.First();
            AssertNewsDto(draftResult, dto);
        }
    }
}
