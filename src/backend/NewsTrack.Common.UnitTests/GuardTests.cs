using System;
using System.Collections.Generic;
using NewsTrack.Common.Validations;
using Xunit;
using FluentAssertions;

namespace NewsTrack.Common.UnitTests
{
    public class GuardTests
    {
        public class Dummy
        {
            public string MyStringProperty { get; set; }
            public object MyObjectProperty { get; set; }
            public Uri MyUriProperty { get; set; }
            public IEnumerable<string> MyEnumProperty { get; set; }
            public IDictionary<string, string> MyDictionaryProperty { get; set; }
        }


        [Fact]
        public void WhenStringIsNull_ThenThrowsArgumentNullException()
        {
            var dummy = new Dummy();
            Action action = () => dummy.MyStringProperty.CheckIfNull(nameof(dummy.MyStringProperty));
            action.Should().Throw<ArgumentNullException>().And.Message.Should().Contain(nameof(Dummy.MyStringProperty));
        }

        [Fact]
        public void WhenStringIsNotNull_ThenDoesNotThrowAnyException()
        {
            var dummy = new Dummy{ MyStringProperty = "this is it" };
            dummy.MyStringProperty.CheckIfNull(nameof(dummy.MyStringProperty));
        }

        [Fact]
        public void WhenObjectIsNull_ThenThrowsArgumentNullException()
        {
            var dummy = new Dummy();
            Action action = () => dummy.MyObjectProperty.CheckIfNull(nameof(dummy.MyObjectProperty));
            action.Should().Throw<ArgumentNullException>().And.Message.Should().Contain(nameof(Dummy.MyObjectProperty));
        }

        [Fact]
        public void WhenObjectIsNotNull_ThenDoesNotThrowAnyException()
        {
            var dummy = new Dummy { MyObjectProperty = new {}};
            dummy.MyObjectProperty.CheckIfNull(nameof(dummy.MyObjectProperty));
        }

        [Fact]
        public void WhenEnumIsNull_ThenThrowsArgumentNullException()
        {
            var dummy = new Dummy();
            Action action = () => dummy.MyEnumProperty.CheckIfNull(nameof(dummy.MyEnumProperty));
            action.Should().Throw<ArgumentNullException>().And.Message.Should().Contain(nameof(Dummy.MyEnumProperty));
        }

        [Fact]
        public void WhenEnumIsNull_ThenDoesNotThrowAnyException()
        {
            var dummy = new Dummy { MyEnumProperty = new [] { "some" }};
            dummy.MyEnumProperty.CheckIfNull(nameof(dummy.MyEnumProperty));
        }

        [Fact]
        public void WhenDictionaryIsNull_ThenThrowsArgumentNullException()
        {
            var dummy = new Dummy();
            Action action = () => dummy.MyDictionaryProperty.CheckIfNull(nameof(dummy.MyEnumProperty));
            action.Should().Throw<ArgumentNullException>().And.Message.Should().Contain(nameof(Dummy.MyEnumProperty));
        }

        [Fact]
        public void WhenDictionaryIsNull_ThenDoesNotThrowAnyException()
        {
            var dummy = new Dummy { MyDictionaryProperty = new Dictionary<string, string>{ { "one", "value1"} } };
            dummy.MyDictionaryProperty.CheckIfNull(nameof(dummy.MyEnumProperty));
        }
    }
}