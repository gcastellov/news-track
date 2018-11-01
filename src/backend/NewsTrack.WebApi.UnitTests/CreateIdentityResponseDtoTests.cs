using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsTrack.Identity.Results;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.UnitTests
{
    [TestClass]
    public class CreateIdentityResponseDtoTests
    {
        [TestMethod]
        public void GivenGoodSaveIdentityResult_WhenMapping_ThenGetSuccess()
        {
            var saveIdResult = SaveIdentityResult.Ok;

            var dto = CreateIdentityResponseDto.Create(saveIdResult);

            Assert.IsTrue(dto.IsSuccessful);
        }

        [DataRow(SaveIdentityResult.InvalidEmail)]
        [DataRow(SaveIdentityResult.InvalidEmailPattern)]
        [DataRow(SaveIdentityResult.InvalidUsername)]
        [DataRow(SaveIdentityResult.PasswordsDontMatch)]
        [TestMethod]
        public void GivenWrongSaveIdentityResult_WhenMapping_ThenGetFailure(SaveIdentityResult saveIdResult)
        {
            var dto = CreateIdentityResponseDto.Create(saveIdResult);

            Assert.IsFalse(dto.IsSuccessful);
            Assert.AreEqual((int)dto.Failure, (int)saveIdResult);
        }
    }
}
