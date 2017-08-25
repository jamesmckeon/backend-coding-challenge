using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeoHub.Logic.Exceptions;
using System.Device.Location;
using System.Diagnostics;

namespace GeoHub.Logic
{
    public class CertaintyRanker : ICertaintyRanker
    {

        protected IEqualityComparer<string> StringComparer { get; set; }
        protected IPercentageRounder PercentageRounder { get; set; }

        public CertaintyRanker() : this(new StringComparer(), new TwoDecimalsRounder()) { }
        public CertaintyRanker(IEqualityComparer<string> stringComparer, IPercentageRounder percentageRounder)
        {
            if (stringComparer == null)
            {
                throw new ArgumentNullException(nameof(stringComparer));
            }

            StringComparer = stringComparer;

            if (percentageRounder == null)
            {
                throw new ArgumentNullException(nameof(percentageRounder));
            }

            PercentageRounder = percentageRounder;
        }

        public const string EitherSourcecoordinatesOrSearchtermMustBeProvidedMessage = "Either sourceCoordinates or searchTerm must be provided";

        /// <summary>
        /// Ranks entries relative to matching name length
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="searchTerm">the text that each entry's name is compared to.</param>
        /// <remarks>ranking is normalized to length of the shortest name that starts with searchTerm</remarks>
        protected IEnumerable<RankingResult> Rank(IEnumerable<GeoDataEntry> entries, string searchTerm)
        {

            var term = searchTerm.Trim().ToLower();
            List<RankingResult> results = null;
            var trimmedEntries = entries
                .Select(e => new { Entry = e, Name = e.Name.Trim().ToLower() })
                .Where(e => e.Name.StartsWith(term));

            //if any entries match the searchTerm at least partially
            if (trimmedEntries.Count() > 0)
            {
                var minLength = trimmedEntries.Min(e => e.Name.Length);

                return
                    trimmedEntries.Select(
                        e => new RankingResult() { Entry = e.Entry, Certainty = PercentageRounder.Round(minLength / (double)e.Name.Length) });

            }


            return results;
        }

        /// <summary>
        /// Ranks entries relative to it's distance from a set of source coordinates
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="sourceCoordinates"></param>
        /// <remarks>ranking is normalized to length of the shortest distance from sourceCoordinates</remarks>
        protected IEnumerable<RankingResult> Rank(IEnumerable<GeoDataEntry> entries, Coordinates sourceCoordinates)
        {

            double minDistance = entries.Select(e => sourceCoordinates.DistanceFrom(e.Latitude, e.Longitude)).Min();

            //if one or more entries are an exact match, divy up 100% certainty between them
            //return zero for all others
            if (minDistance == 0)
            {
                var matchedEntries = entries.Where(e => sourceCoordinates.DistanceFrom(e) == 0);
                var matchedCertainty = PercentageRounder.Round(1 / (double)matchedEntries.Count());

                return
             entries.Select(
                 e =>
                     new RankingResult()
                     {
                         Certainty = matchedEntries.Contains(e) ? matchedCertainty : 0,
                         Entry = e
                     });

            }
            else
            {
                return
             entries.Select(
                 e =>
                     new RankingResult()
                     {
                         Certainty = PercentageRounder.Round( minDistance / sourceCoordinates.DistanceFrom(e.Latitude, e.Longitude)),
                         Entry = e
                     });
            }

        }

        public IEnumerable<RankingResult> Rank(IEnumerable<GeoDataEntry> entries, string searchTerm, Coordinates sourceCoordinates)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            if (entries.Count() == 0)
            {
                throw new ArgumentEmptyException(nameof(entries));
            }

            if (sourceCoordinates == null && string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentException(EitherSourcecoordinatesOrSearchtermMustBeProvidedMessage);
            }

            if (sourceCoordinates != null && !string.IsNullOrEmpty(searchTerm))
            {
                return Rank(entries, searchTerm);
            }
            else if (sourceCoordinates != null)
            {
                return Rank(entries, sourceCoordinates);
            }
            else
            {
                return Rank(entries, searchTerm);
            }

        }
    }
}