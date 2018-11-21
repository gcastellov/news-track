using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsTrack.Common.Validations;

namespace NewsTrack.Common.UnitTests
{
    [TestClass]
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

        [TestMethod]
        public void GivenTypedObject_WhenCheckingForExistingProperty_ThenGetsTrue()
        {
            var value = new TypedObject();
            Assert.IsTrue(value.HasProperty("Name"));
        }

        [TestMethod]
        public void GivenTypedObject_WhenCheckingForNonExistingProperty_ThenGetsFalse()
        {
            var value = new TypedObject();
            Assert.IsFalse(value.HasProperty("LastName"));
        }

        [TestMethod]
        public void GivenDynamicObject_WhenCheckingForExistingProperty_ThenGetsTrue()
        {
            dynamic value = new ExpandoObject();
            value.Name = "some value";
            Assert.IsTrue(((object)value).HasProperty("Name"));
        }

        [TestMethod]
        public void GivenDynamicObject_WhenCheckingForNonExistingProperty_ThenGetsFalse()
        {
            dynamic value = new ExpandoObject();
            Assert.IsFalse(((object)value).HasProperty("Name"));
        }
    }
}