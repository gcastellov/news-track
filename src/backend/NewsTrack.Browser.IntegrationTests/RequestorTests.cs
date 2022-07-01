using System;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;

namespace NewsTrack.Browser.IntegrationTests
{

    public class RequestorTests
    {
        [Fact]
        public async Task WhenRequestingValidUrl_ThenGetResponseAsync()
        {
            var uri = new Uri("https://policies.google.com/terms");
            var requestor = new Requestor();
            var content = await requestor.Get(uri);
            content.Should().NotBeNull();
        }
    }
}
