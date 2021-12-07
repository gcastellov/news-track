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
    public class TopLatestNewsEndpointTests : BaseTest
    {
        private const string Endpoint = "api/news/top/latest";

        public TopLatestNewsEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingNews_WhenGettingTopLatest_ThenReturnsCollection()
        {
            // Arrange
            const uint take = 10;
            var draftResult = CreateDraftEntity();
            var result = new[] { draftResult };
            var endpoint = GetUriWithQueryString(Endpoint, ("take", take));

            Factory.DraftRepositoryMock.Setup(m => m.GetLatest((int)take)).Returns(Task.FromResult(result.AsEnumerable()));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeSuccessful();

            var envelope = await response.ShouldBeOfType<IEnumerable<NewsDigestDto>>();
            envelope.ShouldBeSuccessful();

            envelope.Payload.Should().HaveCount(result.Length);
            var dto = envelope.Payload.First();
            AssertDigestDto(draftResult, dto);
        }

        [Fact]
        public async Task GivenInvalidRequestWithNonPositiveTake_WhenGettingTopLatest_ThenReturnsBadRequest()
        {
            // Arrange
            var endpoint = GetUriWithQueryString(Endpoint, ("take", 0));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeBadRequest();
        }

        [Fact]
        public async Task GivenInvalidRequestWithoutTake_WhenGettingTopLatest_ThenReturnsBadRequest()
        {
            // Act
            var response = await Client.GetAsync(Endpoint);

            // Assert
            response.ShouldBeBadRequest();
        }
    }
}
