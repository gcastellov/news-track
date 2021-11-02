using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Linq;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using NewsTrack.Domain.Exceptions;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class TagsEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/tags";

        public TagsEndpointTests(TestWebAppFactory<Startup> testWebAppFactory)
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenTagsInDb_WhenGettingTags_ThenReturnsAllOfThem()
        {
            // Arrange
            var tags = new[] { "tag1", "tag2" };
            Factory.DraftRepositoryMock.Setup(m => m.GetTags()).Returns(Task.FromResult(tags.AsEnumerable()));

            // Act
            var response = await Client.GetAsync(Endpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<string[]>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.Should().BeEquivalentTo(tags);
        }

        [Fact]
        public async Task GivenNoTagsInDb_WhenGettingTags_ThenReturnsEmpty()
        {
            // Arrange
            Factory.DraftRepositoryMock.Setup(m => m.GetTags()).Returns(Task.FromResult(Enumerable.Empty<string>()));

            // Act
            var response = await Client.GetAsync(Endpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<string[]>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.Should().BeEmpty();
        }

        [Fact]
        public async Task GivenException_WhenGettingTags_ThenReturnsInternalServerError()
        {
            // Arrange
            Factory.DraftRepositoryMock.Setup(m => m.GetTags()).Throws<Exception>();

            // Act
            var response = await Client.GetAsync(Endpoint);

            // Assert
            response.ShouldBeServerError();
        }

        [Fact]
        public async Task GivenNotFoundException_WhenGettingTags_ThenReturnsSuccessful()
        {
            // Arrange
            Factory.DraftRepositoryMock.Setup(m => m.GetTags()).Throws(new NotFoundException(null));

            // Act
            var response = await Client.GetAsync(Endpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<string[]>();
            envelope.ShouldBeUnsuccessful();
        }
    }
}
