using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GeoHub.Services.Data
{
    public class Suggestion
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double  Longitude { get; set; }
        public double Certainty { get; set; }
        /// <summary>
        /// A collection of hypermedia links related to a Suggestion
        /// </summary>
        public IEnumerable<Link> Links { get; set; }

    }
}