using System.Dynamic;
using NewsTrack.Common.Validations;
using Xunit;
using FluentAssertions;

namespace NewsTrack.Common.UnitTests
{
    public class ObjectExtensionsTests
    {
        class TypedObject
        {
            public string Name { get; }

            public TypedObject()
            {
                Name = "Some value";
            }
        }

        [Fact]
        public void GivenTypedObject_WhenCheckingForExistingProperty_ThenGetsTrue()
        {
            var value = new TypedObject();
            value.HasProperty("Name").Should().BeTrue();
        }

        [Fact]
        public void GivenTypedObject_WhenCheckingForNonExistingProperty_ThenGetsFalse()
        {
            var value = new TypedObject();
            value.HasProperty("LastName").Should().BeFalse();
        }

        [Fact]
        public void GivenDynamicObject_WhenCheckingForExistingProperty_ThenGetsTrue()
        {
            dynamic value = new ExpandoObject();
            value.Name = "some value";
            ((object)value).HasProperty("Name").Should().BeTrue();
        }

        [Fact]
        public void GivenDynamicObject_WhenCheckingForNonExistingProperty_ThenGetsFalse()
        {
            dynamic value = new ExpandoObject();
            ((object)value).HasProperty("Name").Should().BeFalse();
        }
    }
}