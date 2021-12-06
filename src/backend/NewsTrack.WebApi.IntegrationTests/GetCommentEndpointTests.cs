using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Threading.Tasks;
using Xunit;
using NewsTrack.WebApi.Dtos;
using NewsTrack.Domain.Exceptions;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class GetCommentEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/comment/{commentId}";

        public GetCommentEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenExistingComment_WhenGettingComment_ThenReturnSuccessfulEnvelope()
        {
            // Arrange
            var comment = CreateCommentEntity();
            var endpoint = Endpoint.Replace("{commentId}", comment.Id.ToString());

            Factory.CommentRepositoryMock.Setup(m => m.Get(comment.Id)).Returns(Task.FromResult(comment));

            // Act
            var response = await Client.GetAsync(endpoint);            

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<CommentDto>();
            envelope.ShouldBeSuccessful();

            AssertCommentDto(comment, envelope.Payload);
        }

        [Fact]
        public async Task GivenNotExistingComment_WhenGettingComment_ThenReturnUnsuccessfulEnvelope()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var endpoint = Endpoint.Replace("{commentId}", commentId.ToString());

            Factory.CommentRepositoryMock.Setup(m => m.Get(commentId)).Throws(new NotFoundException(commentId));

            // Act
            var response = await Client.GetAsync(endpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<CommentDto>();
            envelope.ShouldBeUnsuccessful();
        }
    }
}
