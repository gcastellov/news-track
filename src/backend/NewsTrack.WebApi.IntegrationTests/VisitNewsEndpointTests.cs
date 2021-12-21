using FluentAssertions;
using NewsTrack.WebApi.Dtos;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class VisitNewsEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/news/entry/{id}/visit";

        public VisitNewsEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingNews_WhenVisiting_ThenAddsViewAndGetsAllVisits()
        {
            // Arrange
            const long totalViews = 959;
            var newsId = Guid.NewGuid();
            string endpoint = Endpoint.Replace("{id}", newsId.ToString());

            Factory.DraftRepositoryMock.Setup(m => m.AddViews(newsId)).Returns(Task.FromResult(totalViews));

            // Act
            var response = await Client.PatchAsync(endpoint, null);

            // Assert
            response.ShouldBeSuccessful();

            var envelope = await response.ShouldBeOfType<IncrementalResponseDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.Amount.Should().Be(totalViews);
        }
    }
}
