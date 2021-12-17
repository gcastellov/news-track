using FluentAssertions;
using NewsTrack.WebApi.Dtos;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class LikeCommentEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/comment/{commentId}/like";

        public LikeCommentEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingComment_WhenLikeing_ThenAddsAndGetsAllLikes()
        {
            // Arrange
            const long total = 42;
            var commentId = Guid.NewGuid();
            string endpoint = Endpoint.Replace("{commentId}", commentId.ToString());

            Factory.CommentRepositoryMock.Setup(m => m.AddLike(commentId)).Returns(Task.FromResult(total));

            // Act
            var response = await AuthenticatedPatch(endpoint);

            // Assert
            response.ShouldBeSuccessful();

            var envelope = await response.ShouldBeOfType<IncrementalResponseDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.Amount.Should().Be(total);            
        }
    }
}
