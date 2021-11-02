using NewsTrack.Identity.Results;
using NewsTrack.WebApi.Dtos;
using static NewsTrack.Identity.Results.SaveIdentityResult.ResultType;
using Xunit;
using FluentAssertions;

namespace NewsTrack.WebApi.UnitTests
{
    
    public class CreateIdentityResponseDtoTests
    {
        [Fact]
        public void GivenGoodSaveIdentityResult_WhenMapping_ThenGetSuccess()
        {
            var saveIdResult = Ok;

            var dto = CreateIdentityResponseDto.Create(saveIdResult);

            dto.Failure.Should().BeNull();
        }

        [Theory]
        [InlineData(InvalidEmail)]
        [InlineData(InvalidEmailPattern)]
        [InlineData(InvalidUsername)]
        [InlineData(PasswordsDontMatch)]
        public void GivenWrongSaveIdentityResult_WhenMapping_ThenGetFailure(SaveIdentityResult.ResultType saveIdResult)
        {
            var dto = CreateIdentityResponseDto.Create(saveIdResult);

            dto.Failure.Should().NotBeNull();
            ((int)dto.Failure).Should().Be((int)saveIdResult);
        }
    }
}
