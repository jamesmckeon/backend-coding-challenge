using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoHub.Logic;
using NUnit.Framework;
using Moq;

namespace GeoHub.Logic.Tests
{
    [TestFixture]
    public class BoundingBoxTests
    {

        [Test]
        public void Equals_CoordinatesNotEqual_ReturnsFalse()
        {
            var a = new BoundingBox(new Coordinates() { Latitude = 1.0, Longitude = 1.5 }, 1);
            var b = new BoundingBox(new Coordinates() { Latitude = 1.5, Longitude = 1.5 }, 1);

            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void Equals_RadiusNotEqual_ReturnsFalse()
        {
            var a = new BoundingBox(new Coordinates() { Latitude = 1.5, Longitude = 1.5 }, 1);
            var b = new BoundingBox(new Coordinates() { Latitude = 1.5, Longitude = 1.5 }, 2);

            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void Equals_ReturnsTrue()
        {
            var a = new BoundingBox(new Coordinates() { Latitude = 1.5, Longitude = 1.5 }, 1);
            var b = new BoundingBox(new Coordinates() { Latitude = 1.5, Longitude = 1.5 }, 1);

            Assert.AreEqual(a, b);
        }

        [Test]
        public void GetHasCode_ValuesFlipped_AreNotEqual()
        {
            var a = new BoundingBox(new Coordinates(){Latitude = 1, Longitude = 2}, 1);
            var b = new BoundingBox(new Coordinates(){Latitude = 2, Longitude = 1}, 1);
            Assert.AreNotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }


}
