using NewsTrack.Identity.Results;
using NewsTrack.WebApi.Dtos;
using Xunit;
using FluentAssertions;

namespace NewsTrack.WebApi.UnitTests
{
    
    public class ChangePasswordResponseDtoTests
    {
        [Fact]
        public void GivenGoodChangePasswordResult_WhenMapping_ThenGetSuccess()
        {
            var changePwdResult = ChangePasswordResult.Ok;

            var dto = ChangePasswordResponseDto.Create(changePwdResult);

            dto.Failure.Should().BeNull();
        }

        [Theory]
        [InlineData(ChangePasswordResult.InvalidCurrentPassword)]
        [InlineData(ChangePasswordResult.PasswordsDontMatch)]
        public void GivenWrongChangePasswordResult_WhenMapping_ThenGetFailure(ChangePasswordResult changePwdResult)
        {
            var dto = ChangePasswordResponseDto.Create(changePwdResult);

            dto.Failure.Should().NotBeNull();
            ((int)dto.Failure).Should().Be((int)changePwdResult);
        }
    }
}