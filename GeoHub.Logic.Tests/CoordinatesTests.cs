using System;
using System.Device.Location;
using NUnit.Framework;

namespace GeoHub.Logic.Tests
{
    [TestFixture]
    public class CoordinatesTests
    {
       
        protected  Coordinates Target { get; set; }

        [SetUp]
        public void SetUp()
        {
            Target = new Coordinates();
        }

        [TestCase(.05, 2.6, -.5, 99)]
        [TestCase(-20.05, -.08, -5.5, -.008)]
        public void DistanceFrom_ReturnsExpected(double sourceLatitude, double sourceLongitude, double destLatitude, double destLongitude)
        {
            var source = new GeoCoordinate() {Longitude = sourceLongitude, Latitude = sourceLatitude};
            var dest = new GeoCoordinate() { Longitude = destLongitude, Latitude = destLatitude };

            Target.Longitude = sourceLongitude;
            Target.Latitude = sourceLatitude;

            Assert.AreEqual(source.GetDistanceTo(dest)/1000, Target.DistanceFrom(destLatitude, destLongitude));
        }

        [Test]
        public void FromString_NullLongitude_ThrowsExpected()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Coordinates.FromString(null, "1.1"));

            Assert.AreEqual("latitude", ex.ParamName);
        }

        [Test]
        public void FromString_NullLatitude_ThrowsExpected()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Coordinates.FromString("1.1", null));

            Assert.AreEqual("longitude", ex.ParamName);
        }

        [Test]
        public void FromString_NonNumericLatitude_ThrowsExpected()
        {
            Assert.Throws<ArgumentException>(() => Coordinates.FromString("xxxx", "1.1"));

        }

        [Test]
        public void FromString_NonNumericLongitude_ThrowsExpected()
        {
           Assert.Throws<ArgumentException>(() => Coordinates.FromString("1.1", "xxxxxx"));

        }

        [Test]
        public void Equals_DifferentLongitude_ReturnsFalse()
        {
            var a = new Coordinates() {Longitude = 1, Latitude = 2};
            var b = new Coordinates() {Longitude = -1, Latitude = 2};

            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void Equals_DifferentLatitude_ReturnsFalse()
        {
            var a = new Coordinates() { Longitude = -1, Latitude = 2.01 };
            var b = new Coordinates() { Longitude = -1, Latitude = 2 };

            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void Equals_ReturnsTrue()
        {
            var a = new Coordinates() { Longitude = -1.000000001, Latitude = 2.0000001 };
            var b = new Coordinates() { Longitude = -1.000000001, Latitude = 2.0000001 };

            Assert.AreEqual(a, b);
        }

        [Test]
        public void GetHasCode_ReturnsExpected()
        {
            var a = new Coordinates() { Longitude = -1.000000001, Latitude = 2.0000001 };
            Assert.AreEqual(a.Longitude.GetHashCode() + a.Latitude.GetHashCode(), a.GetHashCode());

        }
    }
}