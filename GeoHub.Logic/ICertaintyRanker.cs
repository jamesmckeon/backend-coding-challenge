using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoHub.Logic
{
    /// <summary>
    /// Implements certainty ranking logic for IGeoDataProvider responses
    /// </summary>
    public interface ICertaintyRanker
    {
        IEnumerable<RankingResult> Rank(IEnumerable<GeoDataEntry> entries, string searchTerm, Coordinates sourceCoordinates) ;
    }
}
