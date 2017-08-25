using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoHub.Services.Data
{
    public class SuggestionQuery
    {
        public string Q { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        /// <summary>
        /// Returns true if all properties are empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Q) && string.IsNullOrEmpty(Latitude) && string.IsNullOrEmpty(Longitude);
        }



       
    }
}