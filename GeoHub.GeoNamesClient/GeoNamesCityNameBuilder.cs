using System;
using System.Linq;

namespace GeoHub.GeoNamesClient
{
    public class GeoNamesCityNameBuilder : IGeoNamesEntryNameBuilder
    {
        public string BuildName(GeonameEntry entry)
        {

            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (string.IsNullOrEmpty(entry.CountryCode))
            {
                return entry.Name;
            }
            else

            if (entry.CountryCode.Equals("US"))
            {
                return string.Format("{0}, {1}, {2}", entry.Name, entry.AdminCode1, entry.CountryCode);
            }
            else
            {
                return string.Format("{0}, {1}, {2}", entry.Name, entry.AdminName1, entry.CountryCode);
            }
        }
    }
}