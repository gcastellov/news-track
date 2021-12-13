using FluentAssertions;
using NewsTrack.WebApi.Dtos;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class DraftCommentEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/comment/news/{draftId}";

        public DraftCommentEndpointTests(TestWebAppFactory<Startup> testWebAppFactory)
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenDraftWithComments_WhenGettingByDraftId_ThenReturnsCommentCollection()
        {
            // Arrange
            const int page = 1;
            const int count = 5;

            var comment = CreateCommentEntity();
            var results = new[] { comment };
            var pathAndQuery = GetUriWithQueryString(
                Endpoint.Replace("{draftId}", comment.DraftId.ToString()),
                ("page", page),
                ("count", count));

            Factory.CommentRepositoryMock.Setup(m => m.GetByDraftId(comment.DraftId, page, count)).Returns(Task.FromResult(results.AsEnumerable()));
            Factory.CommentRepositoryMock.Setup(m => m.CountByDraftId(comment.DraftId)).Returns(Task.FromResult(results.LongLength));

            // Act
            var response = await Client.GetAsync(pathAndQuery);

            // Assert
            response.ShouldBeSuccessful();
            
            var envelope = await response.ShouldBeOfType<CommentsListDto>();
            envelope.ShouldBeSuccessful();

            envelope.Payload.Comments.Should().HaveCount(results.Length);
            envelope.Payload.Count.Should().Be(results.Length);
            var dto = envelope.Payload.Comments.First();
            AssertCommentDto(comment, dto);
        }

        [Fact]
        public async Task GivenInvalidNonPositiveCount_WhenGettingByDraftId_ThenReturnsBadRequest()
        {
            // Arrange
            const int page = 1;
            const int count = 0;

            var pathAndQuery = GetUriWithQueryString(
                Endpoint.Replace("{draftId}", Guid.NewGuid().ToString()),
                ("page", page),
                ("count", count));

            // Act
            var response = await Client.GetAsync(pathAndQuery);

            // Assert
            response.ShouldBeBadRequest();
        }
    }
}
