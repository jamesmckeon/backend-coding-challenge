using NUnit.Framework;

namespace GeoHub.Logic.Tests
{
    [TestFixture]
    public class TwoDecimalsRounderTests
    {
        protected  TwoDecimalsRounder Target { get; set; }

        [SetUp]
        public void Setup()
        {
            Target = new TwoDecimalsRounder();
        }

        [Test]
        public void Round_ReturnsExpected()
        {

            double num = 123.123;
            Assert.AreEqual(123.12, Target.Round(num));
        }
    }
}