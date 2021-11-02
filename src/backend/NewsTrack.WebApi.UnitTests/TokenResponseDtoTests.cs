using NewsTrack.Identity.Results;
using NewsTrack.WebApi.Dtos;
using Xunit;
using FluentAssertions;

namespace NewsTrack.WebApi.UnitTests
{
    
    public class TokenResponseDtoTests
    {
        private static readonly string Username = "my-user";

        [Fact]
        public void GivenGoodAuthenticationResult_WhenMapping_ThenGetSuccess()
        {
            var authResult = AuthenticateResult.Ok;

            var dto = TokenResponseDto.Create(authResult, Username);

            dto.Failure.Should().BeNull();
        }

        [Theory]
        [InlineData(AuthenticateResult.Lockout)]
        [InlineData(AuthenticateResult.Failed)]
        public void GivenWrongAuthenticationResult_WhenMapping_ThenGetFailure(AuthenticateResult authResult)
        {
            var dto = TokenResponseDto.Create(authResult, Username);

            dto.Failure.Should().NotBeNull();
            ((int)dto.Failure).Should().Be((int)authResult);
        }
    }
}