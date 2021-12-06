using FluentAssertions;
using NewsTrack.Domain.Exceptions;
using NewsTrack.WebApi.Dtos;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class CreateCommentEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/comment";

        public CreateCommentEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }


        [Fact]
        public async Task GivenValidContentForExistingNews_WhenCreatingComment_ThenReturnsSuccessfulResponse()
        {
            // Arrange
            var draft = CreateDraftEntity();
            var payload = new CreateCommentDto
            {
                DraftId = draft.Id,
                Content = "some content"
            };

            Factory.DraftRepositoryMock.Setup(m => m.Get(payload.DraftId)).Returns(Task.FromResult(draft));

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<CommentDto>();
            envelope.ShouldBeSuccessful();

            envelope.Payload.Id.Should().NotBeEmpty();
            envelope.Payload.Content.Should().Be(payload.Content);
            envelope.Payload.DraftId.Should().Be(draft.Id);
            envelope.Payload.ReplyingTo.Should().BeNull();
            envelope.Payload.CreatedAt.Should().BeBefore(DateTime.UtcNow);
            envelope.Payload.CreatedBy.Should().Be(Factory.Identity.Username);
            envelope.Payload.Likes.Should().Be(0);
            envelope.Payload.Replies.Should().Be(0);
        }

        [Fact]
        public async Task GivenValidContentForNonExistingNews_WhenCreatingComment_ThenReturnsNotSuccessfulEnvelope()
        {
            // Arrange
            var payload = new CreateCommentDto
            {
                DraftId = Guid.NewGuid(),
                Content = "some content"
            };

            Factory.DraftRepositoryMock.Setup(m => m.Get(payload.DraftId)).Throws(new NotFoundException(payload.DraftId));

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<CommentDto>();
            envelope.ShouldBeUnsuccessful();
        }

        [Fact]
        public async Task GivenValidContentForNonExistingComment_WhenReplyingComment_ThenReturnsNotSuccessfulEnvelope()
        {
            // Arrange
            var draft = CreateDraftEntity();
            var payload = new CreateCommentDto
            {
                DraftId = draft.Id,
                Content = "some content",
                ReplyingTo = Guid.NewGuid()
            };

            Factory.DraftRepositoryMock.Setup(m => m.Get(payload.DraftId)).Returns(Task.FromResult(draft));
            Factory.CommentRepositoryMock.Setup(m => m.Get(payload.ReplyingTo.Value)).Throws(new NotFoundException(payload.ReplyingTo.Value));

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<CommentDto>();
            envelope.ShouldBeUnsuccessful();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(".")]
        public async Task GivenInvalidContent_WhenCreatingComment_ThenReturnsBadRequest(string invalidContent)
        {
            // Arrange
            var payload = new CreateCommentDto
            {
                DraftId = Guid.NewGuid(),
                Content = invalidContent
            };

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeBadRequest();
        }
    }
}
