using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using NewsTrack.WebApi.Dtos;
using NewsTrack.Domain.Entities;
using Moq;
using System;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class DraftEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/draft";

        public DraftEndpointTests(TestWebAppFactory<Program> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenValidDraft_WhenCreatingDraft_ThenSavesItSuccessfully()
        {
            // Arrange
            var payload = CreateDraft();

            Factory.BrowserMock.Setup(m => m.GetContent(payload.Url)).Returns(Task.FromResult("original content"));

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<DraftResponseDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.Id.Should().NotBeEmpty();
            envelope.Payload.Url.Should().Be(payload.Url);

            Factory.DraftRepositoryMock.Verify(m => m.Save(It.IsAny<Draft>()), Times.Once);
            Factory.ContentRepositoryMock.Verify(m => m.Save(It.IsAny<Content>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GivenInvalidDraftWithoutUrl_WhenCreatingDraft_ThenReturnsBadRequest(string url)
        {
            // Arrange
            var payload = CreateDraft();
            payload.Url = url;

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeBadRequest();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GivenInvalidDraftWithoutTitle_WhenCreatingDraft_ThenReturnsBadRequest(string title)
        {
            // Arrange
            var payload = CreateDraft();
            payload.Title = title;

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeBadRequest();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GivenInvalidDraftWithoutPicture_WhenCreatingDraft_ThenReturnsBadRequest(string picture)
        {
            // Arrange
            var payload = CreateDraft();
            payload.Picture = picture;

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeBadRequest();
        }

        [Fact]
        public async Task GivenInvalidDraftWithoutParagraphs_WhenCreatingDraft_ThenReturnsBadRequest()
        {
            // Arrange
            var payload = CreateDraft();
            payload.Paragraphs = null;

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeBadRequest();
        }

        [Fact]
        public async Task GivenInvalidDraftWithEmptyParagraphs_WhenCreatingDraft_ThenReturnsBadRequest()
        {
            // Arrange
            var payload = CreateDraft();
            payload.Paragraphs = Array.Empty<string>();

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeBadRequest();
        }

        private static DraftRequestDto CreateDraft()
            => new DraftRequestDto
            {
                Url = "http://www.some.com/path/resource",
                Paragraphs = new[]
                {
                    "first paragraph",
                    "second paragraph",
                },
                Picture = "http://www.some.com/path/resource/img.png",
                Title = "My first draft",
                Tags = new[] { "first", "second", "third" }
            };
    }
}
