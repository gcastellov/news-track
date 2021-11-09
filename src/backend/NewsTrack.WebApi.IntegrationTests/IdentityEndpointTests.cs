using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class IdentityEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/identity";

        public IdentityEndpointTests(TestWebAppFactory<Startup> testWebAppFactory) 
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenUserIsAuthenticated_WhenGettingIdentity_ThenReturnsIt()
        {
            // Arrange
            Factory.IdentityRepositoryMock.Setup(m => m.Get(Factory.Identity.Id)).Returns(Task.FromResult(Factory.Identity));

            // Act
            var response = await AuthenticatedGet(Endpoint);

            // Assert
            response.ShouldBeSuccessful();
            var envelope = await response.ShouldBeOfType<IdentityDto>();
            envelope.ShouldBeSuccessful();
            
            envelope.Payload.Username.Should().Be(Factory.Identity.Username);
            envelope.Payload.Email.Should().Be(Factory.Identity.Email);
            envelope.Payload.CreatedAt.Should().Be(Factory.Identity.CreatedAt);
            envelope.Payload.IsEnabled.Should().BeTrue();
            envelope.Payload.IdType.Should().Be(Identity.IdentityTypes.Admin);
            envelope.Payload.LastAccessAt.Should().NotBeNull();
            envelope.Payload.LastAccessFailureAt.Should().BeNull();
        }
    }
}
