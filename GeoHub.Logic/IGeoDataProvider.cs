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
        /// <param name="searchTerm">(required) specifies a place name to match on</param>
        /// <param name="maxResults"> (required) the max number of results that should be returned, ordered by relevance/certainty</param>
        /// <returns></returns>
        /// <remarks>matches beginning of place name, case and whitespace insensitive</remarks>
        /// <remarks>Relevance/certainty ranking is implemented by provider</remarks>
     
        IQueryable<GeoDataEntry> Search(string searchTerm, int maxResults);

        /// <summary>
        /// Searches geo data within a bounding box, optionally by name
        /// </summary>
        /// <param name="searchTerm">(optional)</param>
        /// <param name="boundingBox">(required)</param>
        /// /// <param name="maxResults"> (required) the max number of results that should be returned, ordered by relevance/certainty</param>
        /// <returns></returns>
        /// <remarks>matches beginning of place name, case and whitespace insensitive</remarks>
        /// <remarks>Relevance/certainty ranking is implemented by provider</remarks>

        IQueryable<GeoDataEntry> SearchNear(string searchTerm, BoundingBox boundingBox, int maxResults);

    }

}