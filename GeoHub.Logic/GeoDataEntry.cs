using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoHub.Logic
{
    /// <summary>
    /// A result returned by an IGeoDataProvider instance
    /// </summary>
    public class GeoDataEntry: IEquatable<GeoDataEntry>
    {
        public static bool operator ==(GeoDataEntry left, GeoDataEntry right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(GeoDataEntry left, GeoDataEntry right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Name depends on the type of place returned; IGeoDataProviders should implement disambiguation logic that uniquely names GeoDataEntries, as no other unique identifier is specified
        /// </summary>
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Latitude.GetHashCode();
                hashCode = (hashCode * 397) ^ Longitude.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GeoDataEntry) obj);
        }

        public bool Equals(GeoDataEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name) && Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }
    }

}
