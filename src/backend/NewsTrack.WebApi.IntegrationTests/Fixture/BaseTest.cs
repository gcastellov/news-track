using System.Net.Http;
using Xunit;

namespace NewsTrack.WebApi.IntegrationTests.Fixture
{
    public class BaseTest : IClassFixture<TestWebAppFactory<Startup>>
    {
        protected TestWebAppFactory<Startup> Factory { get; }
        protected HttpClient Client { get; }

        protected BaseTest(TestWebAppFactory<Startup> testWebAppFactory)
        {
            Factory = testWebAppFactory;
            Client = Factory.CreateClient();
        }
    }
}
