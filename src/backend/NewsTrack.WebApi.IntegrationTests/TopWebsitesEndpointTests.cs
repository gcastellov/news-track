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
    public class TopWebsitesEndpointTests : BaseTest
    {
        private const string Endpoint = "api/news/top/websites";

        public TopWebsitesEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingNews_WhenGettingTopWebsites_ThenReturnsCollection()
        {
            // Arrange
            const uint take = 10;
            IDictionary<string, long> stats = new Dictionary<string, long>
            {
                { "http://www.some.com", 13243 },
                { "http://www.other.org", 500 }
            };

            var endpoint = GetUriWithQueryString(Endpoint, new Tuple<string, object>("take", take));

            Factory.DraftRepositoryMock.Setup(m => m.GetWebsiteStats((int)take)).Returns(Task.FromResult(stats));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeSuccessful();

            var envelope = await response.ShouldBeOfType<IEnumerable<WebsiteStatsDto>>();
            envelope.ShouldBeSuccessful();

            envelope.Payload.Should().HaveCount(stats.Count);
            foreach(var dto in envelope.Payload)
            {
                stats.Should().ContainKey(dto.Name);
                stats[dto.Name].Should().Be(dto.Count);
            }
        }

        [Fact]
        public async Task GivenInvalidRequestWithNonPositiveTake_WhenGettingTopWebsites_ThenReturnsBadRequest()
        {
            // Arrange
            var endpoint = GetUriWithQueryString(Endpoint, new Tuple<string, object>("take", 0));

            // Act
            var response = await Client.GetAsync(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeBadRequest();
        }

        [Fact]
        public async Task GivenInvalidRequestWithoutTake_WhenGettingTopWebsites_ThenReturnsBadRequest()
        {
            // Act
            var response = await Client.GetAsync(Endpoint);

            // Assert
            response.ShouldBeBadRequest();
        }
    }
}
