using NewsTrack.Common.Validations;
using Xunit;
using FluentAssertions;

namespace NewsTrack.Common.UnitTests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void WhenStringIsNotNull_ThenIsTruthy()
        {
            string value = "some-value";
            value.HasValue().Should().BeTrue();
        }

        [Fact]
        public void WhenStringIsNull_ThenIsFalsy()
        {
            string value = null;
            value.HasValue().Should().BeFalse();
        }

        [Fact]
        public void WhenStringIsEmpty_ThenIsFalsy()
        {
            string value = string.Empty;
            value.HasValue().Should().BeFalse();
        }

        [Fact]
        public void WhenStringIsWhiteSpace_ThenIsFalsy()
        {
            string value = "     ";
            value.HasValue().Should().BeFalse();
        }
    }
}