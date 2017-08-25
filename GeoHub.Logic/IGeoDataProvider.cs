using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoHub.Logic
{
    /// <summary>
    /// A geographical data provider
    /// </summary>
    public interface IGeoDataProvider
    {
        /// <summary>
        /// Searches geo data by name
        /// </summary>
        /// <param name="searchTerm">(optional) specifies a place name to match on</param>
        /// <returns></returns>
        /// <remarks>matches beginning of place name, case and whitespace insensitive</remarks>
     
        IQueryable<GeoDataEntry> Search(string searchTerm);

        /// <summary>
        /// Searches geo data within a bounding box, optionally by name
        /// </summary>
        /// <param name="searchTerm">(optional)</param>
        /// <param name="boundingBox">(required)</param>
        /// <returns></returns>
        /// <remarks>matches beginning of place name, case and whitespace insensitive</remarks>
        IQueryable<GeoDataEntry> SearchNear(string searchTerm, BoundingBox boundingBox);

    }

}