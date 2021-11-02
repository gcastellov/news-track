using System;
using NewsTrack.Domain.Entities;
using Xunit;
using FluentAssertions;

namespace NewsTrack.Domain.UnitTests
{
    
    public class DraftTest
    {
        [Fact]
        public void WhenSeveralKindOfUris_ThenReturnWebsite()
        {
            var uris = new[]
            {
                "http://www.website.com/segment/entry.html",
                "http://www.website.com/segment",
                "http://www.website.com:8080",
                "http://www.website.com:8080/segment",
                "https://www.website.com",
                "https://www.website.com?some=value"
            };

            foreach (var uri in uris)
            {
                var draft = new Draft { Uri = new Uri(uri) };
                draft.Website.Should().Be("www.website.com");
            }
        }
    }
}