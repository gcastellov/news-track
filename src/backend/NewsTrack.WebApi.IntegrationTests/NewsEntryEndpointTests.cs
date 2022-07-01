using System;
using System.Threading.Tasks;
using Xunit;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using NewsTrack.WebApi.Dtos;
using NewsTrack.Domain.Exceptions;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class NewsEntryEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/news/entry/{id}";

        public NewsEntryEndpointTests(TestWebAppFactory<Program> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingNews_WhenGettingIt_ThenReturnsIt()
        {
            // Arrange
            var draftResult = CreateDraftEntity();
            string endpoint = Endpoint.Replace("{id}", draftResult.Id.ToString());
            Factory.DraftRepositoryMock.Setup(m => m.Get(draftResult.Id)).Returns(Task.FromResult(draftResult));

            // Act
            var response = await Client.GetAsync(endpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<NewsDto>();
            envelope.ShouldBeSuccessful();
            AssertNewsDto(draftResult, envelope.Payload);
        }

        [Fact]
        public async Task GivenNonExistingNews_WhenGettingIt_ThenReturnsNotFound()
        {
            // Arrange
            var newsId = Guid.NewGuid();
            string endpoint = Endpoint.Replace("{id}", newsId.ToString());
            Factory.DraftRepositoryMock.Setup(m => m.Get(newsId)).Throws(new NotFoundException(newsId));

            // Act
            var response = await Client.GetAsync(endpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<NewsDto>();
            envelope.ShouldBeUnsuccessful();
        }
    }
}
