using FluentAssertions;
using NewsTrack.WebApi.Dtos;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class FuckNewsEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/news/entry/{id}/fuck";

        public FuckNewsEndpointTests(TestWebAppFactory<Program> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingNews_WhenComplaining_ThenAddsViewAndGetsAllFucks()
        {
            // Arrange
            const long totalViews = 888;
            var newsId = Guid.NewGuid();
            string endpoint = Endpoint.Replace("{id}", newsId.ToString());

            Factory.DraftRepositoryMock.Setup(m => m.AddFuck(newsId)).Returns(Task.FromResult(totalViews));

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
