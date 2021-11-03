using NewsTrack.WebApi.IntegrationTests.Fixture;
using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.Dtos;
using Moq;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class WebsiteEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/website/check";
        private const string ForbiddenUrl = "http://www.some.org/path/resource";

        public WebsiteEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenForbiddenWebsite_WhenCheckingExistence_ThenReturnsForbiddenState()
        {
            // Arrange
            var uri = GetUriWithQueryString(
                Endpoint,
                new Tuple<string, object>("uri", ForbiddenUrl));

            Factory.WebsiteRepositoryMock.Setup(m => m.Exists(It.IsAny<Uri>())).Returns(Task.FromResult(true));

            // Act
            var response = await AuthenticatedGet(uri.PathAndQuery);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<WebsiteDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.IsAccepted.Should().BeFalse();
            envelope.Payload.Uri.Should().Be(ForbiddenUrl);
        }

        [Fact]
        public async Task GivenEmptyForbiddenList_WhenCheckingExistence_ThenReturnsAcceptedState()
        {
            // Arrange
            var uri = GetUriWithQueryString(
                Endpoint,
                new Tuple<string, object>("uri", ForbiddenUrl));

            Factory.WebsiteRepositoryMock.Setup(m => m.Exists(It.IsAny<Uri>())).Returns(Task.FromResult(false));

            // Act
            await Authenticate();
            Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
            var response = await Client.GetAsync(uri.PathAndQuery);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<WebsiteDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.IsAccepted.Should().BeTrue();
            envelope.Payload.Uri.Should().Be(ForbiddenUrl);
        }
    }
}
