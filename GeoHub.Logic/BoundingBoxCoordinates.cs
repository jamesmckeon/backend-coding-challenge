using System;

namespace GeoHub.Logic
{
    /// <summary>
    /// Describes a box that encompasses two geographical coordinates/points
    /// </summary>
    public class BoundingBoxCoordinates
    {
        public Coordinates MinPoint { get; set; }
        public Coordinates MaxPoint { get; set; }
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", MinPoint.Longitude, MinPoint.Latitude, MaxPoint.Longitude, MaxPoint.Latitude);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <remarks>stolen from here:  https://stackoverflow.com/questions/3269202/latitude-and-longitude-bounding-box-for-c </remarks>
        public static BoundingBoxCoordinates GetInKilometers(Coordinates point, double size)
        {
            // Bounding box surrounding the point at given coordinates,
            // assuming local approximation of Earth surface as a sphere
            // of radius given by WGS84
            var lat = Deg2rad(point.Latitude);
            var lon = Deg2rad(point.Longitude);
            var halfSide = 1000 * size;
            // Radius of Earth at given latitude
            var radius = WGS84EarthRadius(lat);
            // Radius of the parallel at given latitude
            var pradius = radius * Math.Cos(lat);
            var latMin = lat - halfSide / radius;
            var latMax = lat + halfSide / radius;
            var lonMin = lon - halfSide / pradius;
            var lonMax = lon + halfSide / pradius;
            return new BoundingBoxCoordinates { MinPoint = new Coordinates { Latitude = Rad2deg(latMin), Longitude = Rad2deg(lonMin) }, MaxPoint = new Coordinates { Latitude = Rad2deg(latMax), Longitude = Rad2deg(lonMax) } };
        }

        // Semi-axes of WGS-84 geoidal reference
        private const double WGS84_a = 6378137.0; // Major semiaxis [m]
        private const double WGS84_b = 6356752.3; // Minor semiaxis [m]
        // 'halfSideInKm' is the half length of the bounding box you want in kilometers.
        // degrees to radians
        private static double Deg2rad(double degrees)
        {
            return Math.PI * degrees / 180.0;
        }

        // radians to degrees
        private static double Rad2deg(double radians)
        {
            return 180.0 * radians / Math.PI;
        }

        // Earth radius at a given latitude, according to the WGS-84 ellipsoid [m]
        private static double WGS84EarthRadius(double lat)
        {
            // http://en.wikipedia.org/wiki/Earth_radius
            var An = WGS84_a * WGS84_a * Math.Cos(lat);
            var Bn = WGS84_b * WGS84_b * Math.Sin(lat);
            var Ad = WGS84_a * Math.Cos(lat);
            var Bd = WGS84_b * Math.Sin(lat);
            return Math.Sqrt((An * An + Bn * Bn) / (Ad * Ad + Bd * Bd));
        }
    }
}