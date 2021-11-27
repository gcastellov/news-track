using FluentAssertions;
using NewsTrack.WebApi.Dtos;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class AuthenticationEndpointTests : BaseTest
    {
        public AuthenticationEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenAccountIsLockedOut_WhenAuthenticating_ThenReturnsUnsuccessfulEnvelope()
        {
            // Arrange
            var identity = Factory.Identity;
            identity.LockoutEnd = System.DateTime.Now.AddDays(1);
            identity.AccessFailedCount = 5;

            var payload = new AuthenticationDto
            {
                Username = identity.Email,
                Password = Factory.IdentityClearPassword
            };

            Factory.IdentityRepositoryMock.Setup(m => m.GetByEmail(payload.Username))
                .Returns(Task.FromResult(identity));

            // Act
            var response = await Client.PostAsJsonAsync(AuthenticationEndpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<TokenResponseDto>();
            envelope.IsSuccessful.Should().BeFalse();
            envelope.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GivenAccountIsNotEnabled_WhenAuthenticating_ThenReturnsUnsuccessfulEnvelope()
        {
            // Arrange
            var identity = Factory.Identity;
            identity.IsEnabled = false; 

            var payload = new AuthenticationDto
            {
                Username = identity.Email,
                Password = Factory.IdentityClearPassword
            };

            Factory.IdentityRepositoryMock.Setup(m => m.GetByEmail(payload.Username))
                .Returns(Task.FromResult(identity));

            // Act
            var response = await Client.PostAsJsonAsync(AuthenticationEndpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<TokenResponseDto>();
            envelope.IsSuccessful.Should().BeFalse();
            envelope.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GivenAccountIsNotFound_WhenAuthenticating_ThenReturnsUnsuccessfulEnvelope()
        {
            // Arrange
            var payload = new AuthenticationDto
            {
                Username = "some@email.com",
                Password = "somepwd"
            };

            Factory.IdentityRepositoryMock.Setup(m => m.GetByEmail(payload.Username))
                .Returns(Task.FromResult<Identity.Identity>(null));

            // Act
            var response = await Client.PostAsJsonAsync(AuthenticationEndpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<TokenResponseDto>();
            envelope.IsSuccessful.Should().BeFalse();
            envelope.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GivenAccountIsAboutToBeLockedOut_WhenAuthenticatingWithInvalidCredentials_ThenGetsLockedAndReturnsUnsuccessfulEnvelope()
        {
            // Arrange
            var identity = Factory.Identity;
            identity.AccessFailedCount = 5;

            var payload = new AuthenticationDto
            {
                Username = identity.Email,
                Password = "invalidpwd"
            };

            Factory.IdentityRepositoryMock.Setup(m => m.GetByEmail(payload.Username))
                .Returns(Task.FromResult(identity));

            // Act
            var response = await Client.PostAsJsonAsync(AuthenticationEndpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<TokenResponseDto>();
            envelope.IsSuccessful.Should().BeFalse();
            envelope.ErrorMessage.Should().BeNullOrEmpty();
            identity.LockoutEnd.Should().BeAfter(System.DateTime.UtcNow);
        }
    }
}
