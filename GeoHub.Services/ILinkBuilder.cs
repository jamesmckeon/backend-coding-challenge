using System.Collections.Generic;
using GeoHub.Logic;
using GeoHub.Services.Data;

namespace GeoHub.Services
{
    /// <summary>
    /// Generates Hypermedia links for a GeoDataEntry
    /// </summary>
    public interface ILinkBuilder
    {
        IEnumerable<Link> BuildLinks(GeoDataEntry entry);
    }
}