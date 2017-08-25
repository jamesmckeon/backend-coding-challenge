using System;

namespace GeoHub.Logic
{
    /// <summary>
    /// Rounds decimal/double values to two decimals
    /// </summary>
    public class TwoDecimalsRounder : IPercentageRounder
    {
        public double Round(double val)
        {
            return Math.Round(val, 2);
        }
    }
}