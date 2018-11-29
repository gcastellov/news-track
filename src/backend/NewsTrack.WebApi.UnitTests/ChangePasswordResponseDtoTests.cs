using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsTrack.Identity.Results;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.UnitTests
{
    [TestClass]
    public class ChangePasswordResponseDtoTests
    {
        [TestMethod]
        public void GivenGoodChangePasswordResult_WhenMapping_ThenGetSuccess()
        {
            var changePwdResult = ChangePasswordResult.Ok;

            var dto = ChangePasswordResponseDto.Create(changePwdResult);

            Assert.IsNull(dto.Failure);
        }

        [DataRow(ChangePasswordResult.InvalidCurrentPassword)]
        [DataRow(ChangePasswordResult.PasswordsDontMatch)]
        [TestMethod]
        public void GivenWrongChangePasswordResult_WhenMapping_ThenGetFailure(ChangePasswordResult changePwdResult)
        {
            var dto = ChangePasswordResponseDto.Create(changePwdResult);

            Assert.IsNotNull(dto.Failure);
            Assert.AreEqual((int)dto.Failure, (int)changePwdResult);
        }
    }
}