using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using NewsTrack.Browser.Dtos;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class BrowseEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/browser/browse";

        public BrowseEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }
        
        [Fact]
        public async Task GivenValidUrl_WhenBrowsing_ThenReturnsBrowseResult()
        {
            // Arrange
            var url = new Uri("http://www.some.com/path/resource");
            var endpoint = GetUriWithQueryString(Endpoint, ("url", url.AbsoluteUri));
            var browserDto = new ResponseDto(url)
            {
                Titles = new[]
                {
                    "First title",
                    "Second title",
                },
                Paragraphs = new[]
                {
                    "First paragraph",
                    "Second paragraph",
                },
                Pictures = new[]
                {
                    new Uri("http://www.some.com/path/resource/img1.png"),
                    new Uri("http://www.some.com/path/resource/img2.png"),
                },
            };

            Factory.BrowserMock.Setup(m => m.Get(url.AbsoluteUri)).Returns(Task.FromResult(browserDto));

            // Act
            var response = await AuthenticatedGet(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<BrowseResponseDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.Uri.Should().Be(url);
            envelope.Payload.Paragraphs.Should().BeEquivalentTo(browserDto.Paragraphs);
            envelope.Payload.Pictures.Should().BeEquivalentTo(browserDto.Pictures);
            envelope.Payload.Titles.Should().BeEquivalentTo(browserDto.Titles);
        }

        [Fact]
        public async Task GivenInvalidUrl_WhenBrowsing_ThenReturnsBadRequest()
        {
            // Arrange
            var endpoint = GetUriWithQueryString(Endpoint, ("url", "wrong_url"));

            // Act
            var response = await AuthenticatedGet(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeBadRequest();
        }

        [Fact]
        public async Task GivenInvalidEmptyUrl_WhenBrowsing_ThenReturnsBadRequest()
        {
            // Arrange
            var endpoint = GetUriWithQueryString(Endpoint, ("url", null));

            // Act
            var response = await AuthenticatedGet(endpoint.PathAndQuery);

            // Assert
            response.ShouldBeBadRequest();
        }
    }
}
