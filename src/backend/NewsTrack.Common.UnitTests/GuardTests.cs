using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NewsTrack.Common.Validations;

namespace NewsTrack.Common.UnitTests
{
    [TestClass]
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


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), nameof(Dummy.MyStringProperty))]
        public void WhenStringIsNull_ThenThrowsArgumentNullException()
        {
            var dummy = new Dummy();
            dummy.MyStringProperty.CheckIfNull(nameof(dummy.MyStringProperty));
        }

        [TestMethod]
        public void WhenStringIsNotNull_ThenDoesNotThrowAnyException()
        {
            var dummy = new Dummy{ MyStringProperty = "this is it" };
            dummy.MyStringProperty.CheckIfNull(nameof(dummy.MyStringProperty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), nameof(Dummy.MyObjectProperty))]
        public void WhenObjectIsNull_ThenThrowsArgumentNullException()
        {
            var dummy = new Dummy();
            dummy.MyObjectProperty.CheckIfNull(nameof(dummy.MyObjectProperty));
        }

        [TestMethod]
        public void WhenObjectIsNotNull_ThenDoesNotThrowAnyException()
        {
            var dummy = new Dummy { MyObjectProperty = new {}};
            dummy.MyObjectProperty.CheckIfNull(nameof(dummy.MyObjectProperty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), nameof(Dummy.MyEnumProperty))]
        public void WhenEnumIsNull_ThenThrowsArgumentNullException()
        {
            var dummy = new Dummy();
            dummy.MyEnumProperty.CheckIfNull(nameof(dummy.MyEnumProperty));
        }

        [TestMethod]
        public void WhenEnumIsNull_ThenDoesNotThrowAnyException()
        {
            var dummy = new Dummy { MyEnumProperty = new [] { "some" }};
            dummy.MyEnumProperty.CheckIfNull(nameof(dummy.MyEnumProperty));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), nameof(Dummy.MyDictionaryProperty))]
        public void WhenDictionaryIsNull_ThenThrowsArgumentNullException()
        {
            var dummy = new Dummy();
            dummy.MyDictionaryProperty.CheckIfNull(nameof(dummy.MyEnumProperty));
        }

        [TestMethod]
        public void WhenDictionaryIsNull_ThenDoesNotThrowAnyException()
        {
            var dummy = new Dummy { MyDictionaryProperty = new Dictionary<string, string>{ { "one", "value1"} } };
            dummy.MyDictionaryProperty.CheckIfNull(nameof(dummy.MyEnumProperty));
        }
    }
}