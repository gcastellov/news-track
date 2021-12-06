using FluentAssertions;
using NewsTrack.WebApi.Dtos;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class CommentRepliesEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/comment/{commentId}/replies";
        
        public CommentRepliesEndpointTests(TestWebAppFactory<Startup> testWebAppFactory)
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenCommentAndReplies_WhenGettingReplies_ThenReturnsCollection()
        {
            // Arrange
            const int page = 1;
            const int count = 5;

            var comment = CreateCommentEntity();
            var replies = new[] { comment };
            var pathAndQuery = GetUriWithQueryString(
                Endpoint.Replace("{commentId}", comment.ReplyingTo.ToString()),
                new Tuple<string, object>("page", page),
                new Tuple<string, object>("count", count));

            Factory.CommentRepositoryMock.Setup(m => m.GetReplies(comment.ReplyingTo.Value, page, count)).Returns(Task.FromResult(replies.AsEnumerable()));

            // Act
            var response = await Client.GetAsync(pathAndQuery);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<IEnumerable<CommentDto>>();
            envelope.ShouldBeSuccessful();

            envelope.Payload.Should().HaveCount(replies.Length);
            var dto = envelope.Payload.First();
            AssertCommentDto(comment, dto);
        }

        [Fact]
        public async Task GivenInvalidNonPositiveCount_WhenGettingReplies_GetsBadRequest()
        {
            // Arrange
            const int page = 1;
            const int count = 0;

            var pathAndQuery = GetUriWithQueryString(
                Endpoint.Replace("{commentId}", Guid.NewGuid().ToString()),
                new Tuple<string, object>("page", page),
                new Tuple<string, object>("count", count));

            // Act
            var response = await Client.GetAsync(pathAndQuery);

            // Assert
            response.ShouldBeBadRequest();
        }
    }
}
