using NUnit.Framework;

namespace GeoHub.Logic.Tests
{
    [TestFixture]
    public class GeoDataEntryTests
    {

        [Test]
        public void Equals_DifferentNames_AreNotEqual()
        {
            var a = new GeoDataEntry() {Name = "A", Latitude = 1.0, Longitude = 2.0};
            var b = new GeoDataEntry() { Name = "B", Latitude = 1.0, Longitude = 2.0 };
            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void Equals_DifferentLatitude_AreNotEqual()
        {
            var a = new GeoDataEntry() { Name = "A", Latitude = 1.01, Longitude = 2.0 };
            var b = new GeoDataEntry() { Name = "A", Latitude = 1.0, Longitude = 2.0 };
            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void Equals_DifferentLongitude_AreNotEqual()
        {
            var a = new GeoDataEntry() { Name = "A", Latitude = 1.0, Longitude = 2.0 };
            var b = new GeoDataEntry() { Name = "A", Latitude = 1.0, Longitude = 2.01 };
            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void Equals_AreEqual_ReturnEqual()
        {
            var a = new GeoDataEntry() { Name = "A", Latitude = 1.0, Longitude = 2.0 };
            var b = new GeoDataEntry() { Name = "A", Latitude = 1.0, Longitude = 2.0 };
            Assert.AreEqual(a, b);
        }

        [Test]
        public void GetHasCode_ReturnsExpected()
        {
            var a = new GeoDataEntry() { Name = "A", Latitude = 1.0, Longitude = 2.0 };
          
            Assert.AreEqual(a.Name.GetHashCode() + a.Latitude.GetHashCode() + a.Longitude.GetHashCode(), a.GetHashCode());
        }
    }
}