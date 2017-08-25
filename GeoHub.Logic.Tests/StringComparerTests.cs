using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using  Moq;
using GeoHub.Logic;

namespace GeoHub.Logic.Tests
{
    [TestFixture]
    public class StringComparerTests
    {

        protected  StringComparer Target { get; set; }

        [SetUp]
        public void TestSetUp()
        {
            Target = new StringComparer();
        }

        [Test]
        public void GetHashCode_ReturnsExpected()
        {
            const string testString = "    Aa1Z z # t             ";
            Assert.AreEqual(testString.Trim().ToLower().GetHashCode(), Target.GetHashCode(testString));
        }

        [Test]
        public void GetHashCode_NullString_ReturnsExpected()
        {
      
            Assert.AreEqual(string.Empty.GetHashCode(), Target.GetHashCode(null));
        }

        [Test]
        public void Equals_BothNull_ReturnsTrue()
        {
            Assert.IsTrue(Target.Equals(null, null));
        }

        [Test]
        public void Equals_BothEmpty_ReturnsTrue()
        {
            Assert.IsTrue(Target.Equals("          ", " "));
        }

        [Test]
        public void Equals_EqualStrings_IgnoresCasingAndWhitespaces()
        {

            const string a = " a A b B ";
            const string b = " A a B b ";
            Assert.IsTrue(Target.Equals(a, b));
        }
    }
}
