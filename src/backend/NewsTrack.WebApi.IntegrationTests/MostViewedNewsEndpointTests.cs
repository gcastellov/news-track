using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class MostViewedNewsEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/news/mostviewed";

        public MostViewedNewsEndpointTests(TestWebAppFactory<Startup> testWebAppFactory)
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingNews_WhenGettingMostViewed_ThenReturnsCollection()
        {
            // Arrange
            const uint page = 0;
            const uint count = 10;

            var draftResult = CreateDraftEntity();
            var results = new[] { draftResult };

            var endpoint = GetUriWithQueryString(
                Endpoint,
                ("page", page),
                ("count", count));

            Factory.DraftRepositoryMock.Setup(m => m.GetMostViewed((int)page, (int)count)).Returns(Task.FromResult(results.AsEnumerable()));
            Factory.DraftRepositoryMock.Setup(m => m.Count()).Returns(Task.FromResult((long)results.Length));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<NewsResponseListDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.News.Should().HaveCount(results.Length);
            envelope.Payload.Count.Should().Be((long)results.Length);

            var dto = envelope.Payload.News.First();
            AssertNewsDto(draftResult, dto);
        }
    }
}
