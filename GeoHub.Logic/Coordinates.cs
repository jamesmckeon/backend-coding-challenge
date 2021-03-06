using System;
using System.Device.Location;

namespace GeoHub.Logic
{
    /// <inheritdoc />
    /// <summary>
    /// A geographical point
    /// </summary>
    /// <remarks>Could've used GeoCoordinate class in .Net, but I didn't realize it existed until late in the game</remarks>
    public class Coordinates: IEquatable< Coordinates>
    {
        public static bool operator ==(Coordinates left, Coordinates right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Coordinates left, Coordinates right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// returns Coordinates for NYC
        /// </summary>
        /// <returns></returns>
        public static Coordinates NewYork()
        {
            return  new Coordinates() { Latitude = 40.730610, Longitude = -73.935242 };
        }

        public static Coordinates FromString(string latitude, string longitude)
        {
            if (string.IsNullOrEmpty(latitude))
            {
                throw new ArgumentNullException(nameof(latitude));
            }

            if ( string.IsNullOrEmpty(longitude))
            {
                throw new ArgumentNullException(nameof(longitude));
            }

            double la;
            double lo;

            if (!double.TryParse(latitude, out la))
            {
                throw new ArgumentException("latitude cannot be converted to a decimal");
            }

            if (!double.TryParse(longitude, out lo))
            {
                throw new ArgumentException("longitude cannot be converted to a decimal");
            }

            return new Coordinates() {Latitude = la, Longitude = lo};
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        /// <summary>
        /// Calculates the distance between two coordinates, in kilometers.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public double DistanceFrom(double latitude, double longitude)
        {
            return
                new GeoCoordinate() { Latitude = this.Latitude, Longitude = this.Longitude }.GetDistanceTo(
                    new GeoCoordinate() { Latitude = latitude, Longitude = longitude })/1000;
        }

        /// <summary>
        /// Calculates the distance between two coordinate and a GeoDataEntry instance, in meters.
        /// </summary>
        /// <param name="geoDataEntry"></param>
        /// <returns></returns>
        public double DistanceFrom(GeoDataEntry geoDataEntry)
        {

            if (geoDataEntry == null)
            {
                throw new ArgumentNullException(nameof(geoDataEntry));
            }

            return DistanceFrom(geoDataEntry.Latitude, geoDataEntry.Longitude);

        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Coordinates) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode();
            }
        }

        public bool Equals(Coordinates other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }
    }
}