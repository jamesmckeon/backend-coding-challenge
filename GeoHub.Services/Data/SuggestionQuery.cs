using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoHub.Logic;

namespace GeoHub.Services.Data
{
    public class SuggestionQuery
    {
        public string Q { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int MaxResults { get; set; } = 10;

        /// <summary>
        /// Validates query string, throws exceptions if any errors are found
        /// </summary>
        /// <returns></returns>
        public void Validate()
        {
            //if Q isn't provided, both latitude and longitude must be
            if (string.IsNullOrEmpty(Q))
            {
                if (!Latitude.HasValue)
                {
                    throw new ArgumentNullException("Latitude");
                }
                else if (!Longitude.HasValue)
                {
                    throw new ArgumentNullException("Longitude");
                }
            }
        }

        /// <summary>
        /// Instantiates a Coordinates object using Latitude and Longitude, if present
        /// </summary>
        /// <returns></returns>
        public Coordinates Coordinates()
        {
    
            if (Longitude.HasValue && Latitude.HasValue)
            {
                return new Coordinates() { Longitude = Longitude.Value, Latitude = Latitude.Value };
            }
            else
            {
                return null;
            }
     
        }

    }


}