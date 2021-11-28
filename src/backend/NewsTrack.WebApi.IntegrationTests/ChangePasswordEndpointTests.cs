using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NewsTrack.WebApi.IntegrationTests.Fixture;
using NewsTrack.WebApi.Dtos;
using NewsTrack.Identity.Results;

namespace NewsTrack.WebApi.IntegrationTests
{
    public class ChangePasswordEndpointTests : BaseTest
    {
        private const string Endpoint = "/api/identity/password/change";
        
        public ChangePasswordEndpointTests(TestWebAppFactory<Startup> testWebAppFactory)
            : base(testWebAppFactory)
        {
        }

        [Fact]
        public async Task GivenUserIsAuthenticatedAndPasswordMatches_WhenChangingPassword_ThenSavesNewPassword()
        {
            // Arrange
            var payload = new ChangePasswordDto
            {
                CurrentPassword = Factory.IdentityClearPassword,
                ConfirmPassword = "newpassword",
                Password = "newpassword",
            };

            Factory.IdentityRepositoryMock.Setup(m => m.Get(Factory.Identity.Id)).Returns(Task.FromResult(Factory.Identity));

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();

            var envelope = await response.ShouldBeVoid();
            envelope.ShouldBeSuccessful();
        }

        [Fact]
        public async Task GivenUserIsAuthenticatedAndPasswordDoesNotMatch_WhenChangingPassword_ThenReturnsError()
        {
            // Arrange
            var payload = new ChangePasswordDto
            {
                CurrentPassword = "wrongpassword",
                ConfirmPassword = "newpassword",
                Password = "newpassword",
            };

            Factory.IdentityRepositoryMock.Setup(m => m.Get(Factory.Identity.Id)).Returns(Task.FromResult(Factory.Identity));

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();

            var envelope = await response.ShouldBeVoid();
            envelope.ShouldBeUnsuccessful();
            envelope.Error.Code.Should().Be((uint)ChangePasswordResult.InvalidCurrentPassword);
        }

        [Fact]
        public async Task GivenUserIsAuthenticatedAndConfirmationPasswordDoesNotMatch_WhenChangingPassword_ThenReturnsError()
        {
            // Arrange
            var payload = new ChangePasswordDto
            {
                CurrentPassword = Factory.IdentityClearPassword,
                ConfirmPassword = "newwrongpassword",
                Password = "newpassword",
            };

            Factory.IdentityRepositoryMock.Setup(m => m.Get(Factory.Identity.Id)).Returns(Task.FromResult(Factory.Identity));

            // Act
            var response = await AuthenticatedPost(Endpoint, payload);

            // Assert
            response.ShouldBeSuccessful();

            var envelope = await response.ShouldBeVoid();
            envelope.ShouldBeUnsuccessful();
            envelope.Error.Code.Should().Be((uint)ChangePasswordResult.PasswordsDontMatch);
        }
    }
}
