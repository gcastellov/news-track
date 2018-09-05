using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsTrack.Domain.Entities;

namespace NewsTrack.Domain.UnitTests
{
    [TestClass]
    public class DraftTest
    {
        [TestMethod]
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
                Assert.AreEqual(draft.Website, "www.website.com");
            }
        }
    }
}