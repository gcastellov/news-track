using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsTrack.Identity.Results;
using NewsTrack.WebApi.Dtos;
using static NewsTrack.Identity.Results.SaveIdentityResult.ResultType;

namespace NewsTrack.WebApi.UnitTests
{
    [TestClass]
    public class CreateIdentityResponseDtoTests
    {
        [TestMethod]
        public void GivenGoodSaveIdentityResult_WhenMapping_ThenGetSuccess()
        {
            var saveIdResult = Ok;

            var dto = CreateIdentityResponseDto.Create(saveIdResult);

            Assert.IsNull(dto.Failure);
        }

        [DataRow(InvalidEmail)]
        [DataRow(InvalidEmailPattern)]
        [DataRow(InvalidUsername)]
        [DataRow(PasswordsDontMatch)]
        [TestMethod]
        public void GivenWrongSaveIdentityResult_WhenMapping_ThenGetFailure(SaveIdentityResult.ResultType saveIdResult)
        {
            var dto = CreateIdentityResponseDto.Create(saveIdResult);

            Assert.IsNotNull(dto.Failure);
            Assert.AreEqual((int)dto.Failure, (int)saveIdResult);
        }
    }
}
