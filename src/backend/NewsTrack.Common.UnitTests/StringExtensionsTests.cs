using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsTrack.Common.Validations;

namespace NewsTrack.Common.UnitTests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void WhenStringIsNotNull_ThenIsTruthy()
        {
            string value = "some-value";
            Assert.IsTrue(value.HasValue());
        }

        [TestMethod]
        public void WhenStringIsNull_ThenIsFalsy()
        {
            string value = null;
            Assert.IsFalse(value.HasValue());
        }

        [TestMethod]
        public void WhenStringIsEmpty_ThenIsFalsy()
        {
            string value = string.Empty;
            Assert.IsFalse(value.HasValue());
        }

        [TestMethod]
        public void WhenStringIsWhiteSpace_ThenIsFalsy()
        {
            string value = "     ";
            Assert.IsFalse(value.HasValue());
        }
    }
}