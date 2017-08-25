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
        /// <summary>
        /// Name depends on the type of place returned; IGeoDataProviders should implement disambiguation logic that uniquely names GeoDataEntries, as no other unique identifier is specified
        /// </summary>
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override int GetHashCode()
        {
            return Name.GetHashCode() + Latitude.GetHashCode() + Longitude.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GeoDataEntry);
        }

        public bool Equals(GeoDataEntry other)
        {
            if (other == null)
            {
                return false;
            }

            else
            {

                return other.Name.Equals(Name) & other.Latitude == Latitude && other.Longitude == Longitude;
            }
        }
    }

}
