using NewsTrack.WebApi.Dtos;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class SignupEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/identity/signup";

        public SignupEndpointTests(TestWebAppFactory<Startup> testWebAppFactory)
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenValidEmailAndUsername_WhenSigningUp_ThenAccountIsCreatedAndReturnsSuccessfulEnvelope()
        {
            // Arrange
            var payload = new CreateIdentityDto
            {
                Email = "some@email.com",
                Username = "some"
            };

            // Act
            var response = await Client.PostAsJsonAsync(Endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<CreateIdentityResponseDto>();
            envelope.ShouldBeSuccessful();
        }

        [Fact]
        public async Task GivenExistingEmail_ThenSigningUp_ThenReturnsUnsuccessfulEnvelope()
        {
            // Arrange
            var payload = new CreateIdentityDto
            {
                Email = "some@email.com",
                Username = "some"
            };

            Factory.IdentityRepositoryMock.Setup(m => m.ExistsByEmail(payload.Email))
                .Returns(Task.FromResult(true));

            // Act
            var response = await Client.PostAsJsonAsync(Endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<CreateIdentityResponseDto>();
            envelope.ShouldBeUnsuccessful();
        }
    }
}
