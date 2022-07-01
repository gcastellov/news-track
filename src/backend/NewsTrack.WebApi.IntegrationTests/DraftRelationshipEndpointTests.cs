using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using NewsTrack.WebApi.Dtos;
using Moq;
using NewsTrack.Domain.Entities;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class DraftRelationshipEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/draft/{id}/relationship";

        public DraftRelationshipEndpointTests(TestWebAppFactory<Program> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingDrafts_WhenSettingRelationship_ThenSavesAndLinkThem()
        {
            // Arrange
            var draftId = Guid.NewGuid();
            var payload = new[]
            {
                new NewsDigestBaseDto
                {
                    Id = Guid.NewGuid(),
                    Title = "First draft title",
                    Url = new Uri("http://www.first.com/path/resource")
                },
                new NewsDigestBaseDto
                {
                    Id = Guid.NewGuid(),
                    Title = "Second draft title",
                    Url = new Uri("http://www.second.com/path/resource")
                },
            };

            string endpoint = Endpoint.Replace("{id}", draftId.ToString());

            // Act
            var response = await AuthenticatedPost(endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<DraftRelationshipResponseDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.Id.Should().Be(draftId);

            Factory.DraftRelationshipRepositoryMock.Verify(m => m.SetRelationship(It.Is<DraftRelationship>(d => d.Id == draftId)), Times.Once());
            Factory.DraftRepositoryMock.Verify(m => m.SetRelationship(draftId, payload.Length), Times.Once());
        }
    }
}
