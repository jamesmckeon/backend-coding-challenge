namespace GeoHub.Logic
{
    /// <summary>
    /// The result of a certainty ranking operation
    /// </summary>
    /// <remarks>See <seealso cref="ICertaintyRanker"/></remarks>
    public class RankingResult 
    {
        public GeoDataEntry Entry { get; set; }
        public double Certainty { get; set; }
    }
}