using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewsTrack.Browser.IntegrationTests
{
    [TestClass]
    public class RequestorTests
    {
        [TestMethod]
        public async Task WhenRequestingValidUrl_ThenGetResponseAsync()
        {
            var uri = new Uri("https://help.github.com/articles/github-terms-of-service/");
            var requestor = new Requestor();
            var content = await requestor.Get(uri);
            Assert.IsNotNull(content);
        }
    }
}
