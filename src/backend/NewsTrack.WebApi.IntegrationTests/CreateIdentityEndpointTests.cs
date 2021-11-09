using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class CreateIdentityEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/identity/create";

        public CreateIdentityEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenUserIsAuthenticated_WhenCreating_ThenCreatesIdentitySuccessfully()
        {
            // Arrange
            var payload = new CreateIdentityDto
            {
                Email = "newuser@address.com",
                Username = "newuser",
            };

            Factory.IdentityRepositoryMock.Setup(m => m.ExistsByUsername(payload.Username)).Returns(Task.FromResult(false));
            Factory.IdentityRepositoryMock.Setup(m => m.ExistsByEmail(payload.Email)).Returns(Task.FromResult(false));

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<CreateIdentityResponseDto>();
            envelope.ShouldBeSuccessful();
            envelope.Payload.Failure.Should().BeNull();
        }

        [Theory]
        [InlineData("newuser.com", "newuser")]
        [InlineData("newuser@address.com", "")]
        [InlineData("", "newuser")]
        [InlineData("newuser@address.com", null)]
        [InlineData(null, "newuser")]
        [InlineData(null, null)]
        public async Task GivenParametersWithWrongFormat_WhenCreating_ThenReturnsBadRequest(string email, string password)
        {
            // Arrange
            var payload = new CreateIdentityDto
            {
                Email = email,
                Username = password,
            };

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeBadRequest();
        }
    }
}
