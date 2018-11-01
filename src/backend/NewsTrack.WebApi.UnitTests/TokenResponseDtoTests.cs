using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nest;
using NewsTrack.Identity.Results;
using NewsTrack.WebApi.Dtos;

namespace NewsTrack.WebApi.UnitTests
{
    [TestClass]
    public class TokenResponseDtoTests
    {
        private static readonly string Username = "my-user";

        [TestMethod]
        public void GivenGoodAuthenticationResult_WhenMapping_ThenGetSuccess()
        {
            var authResult = AuthenticateResult.Ok;

            var dto = TokenResponseDto.Create(authResult, Username);

            Assert.IsTrue(dto.IsSuccessful);
        }

        [DataRow(AuthenticateResult.Lockout)]
        [DataRow(AuthenticateResult.Failed)]
        [TestMethod]
        public void GivenWrongAuthenticationResult_WhenMapping_ThenGetFailure(AuthenticateResult authResult)
        {
            var dto = TokenResponseDto.Create(authResult, Username);

            Assert.IsFalse(dto.IsSuccessful);
            Assert.AreEqual((int)dto.Failure, (int)authResult);
        }
    }
}