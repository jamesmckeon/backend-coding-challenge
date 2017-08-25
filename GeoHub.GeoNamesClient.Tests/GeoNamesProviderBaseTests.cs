using GeoHub.Logic;
using NUnit.Framework;

namespace GeoHub.GeoNamesClient.Tests
{
    [TestFixture]
    public class GeoNamesProviderBaseTests
    {
        [Test]
        public void GetCoordinateArgs_ReturnsExpected()
        {

            var box = new BoundingBox(Coordinates.NewYork(), 50);

            var coordinates = BoundingBoxCoordinates.GetInKilometers(box.Point, box.Radius);
            var expected = string.Format("north={0}&south={1}&east={2}&west={3}", coordinates.MaxPoint.Latitude,
                coordinates.MinPoint.Latitude, coordinates.MaxPoint.Longitude, coordinates.MinPoint.Longitude);


            Assert.AreEqual(expected, GeoNamesProviderBase.GetCoordinateArgs(box));
        }
    }
}