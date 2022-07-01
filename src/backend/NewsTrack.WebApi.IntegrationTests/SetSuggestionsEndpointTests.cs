using System.Threading.Tasks;
using Xunit;
using NewsTrack.WebApi.IntegrationTests.Fixture;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class SetSuggestionsEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/content/suggestions";

        public SetSuggestionsEndpointTests(TestWebAppFactory<Program> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenBeingAuthenticated_WhenTriggeringSuggestions_ThenReturnsAcceptedResult()
        {
            // Act
            var response = await AuthenticatedPost(Endpoint);

            // Assert
            response.ShouldBeAccepted();
        }
    }
}
