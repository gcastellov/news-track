﻿using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace NewsTrack.Browser.IntegrationTests
{
    public class BrowserTests
    {
        [Fact]
        public async Task WhenRequestingValidUrl_ThenGetResponseDto()
        {
            var requestor = new Requestor();
            var browser = new Broswer(requestor);
            var response = await browser.Get("https://policies.google.com/terms");

            response.Should().NotBeNull();
            response.Titles.Should().NotBeEmpty();
            response.Paragraphs.Should().NotBeEmpty();
            response.Pictures.Should().NotBeEmpty();
        }
    }
}