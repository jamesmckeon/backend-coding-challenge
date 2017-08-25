using System.Collections.Generic;
using GeoHub.Logic;
using GeoHub.Services.Data;

namespace GeoHub.Services
{
    /// <summary>
    /// Maps the results of a IGeoDataProvider query to data returned by the Api/Controllers
    /// </summary>
    public interface IResultsMapper
    {
        SuggestionQueryResult Map(IEnumerable<GeoDataEntry> results, SuggestionQuery suggestionQuery, ILinkBuilder linkBuilder);
    }
}