using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NewsTrack.Browser.IntegrationTests
{
    [TestClass]
    public class BrowserTests
    {
        [TestMethod]
        public async Task WhenRequestingValidUrl_ThenGetResponseDto()
        {
            var requestor = new Requestor();
            var browser = new Broswer(requestor);
            var response = await browser.Get("https://help.github.com/articles/github-terms-of-service/");

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Titles.Any());
            Assert.IsTrue(response.Paragraphs.Any());
            Assert.IsTrue(response.Pictures.Any());
        }
    }
}