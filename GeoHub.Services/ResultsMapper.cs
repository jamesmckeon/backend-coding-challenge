using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;
//using System.Security.Policy;
using GeoHub.Logic;
using GeoHub.Services.Data;

namespace GeoHub.Services
{
    public class ResultsMapper : IResultsMapper
    {
        protected ICertaintyRanker CertaintyRanker { get; set; }

        public ResultsMapper(ICertaintyRanker certaintyRanker)
        {
            if (certaintyRanker == null)
            {
                throw new ArgumentNullException(nameof(certaintyRanker));
            }

            CertaintyRanker = certaintyRanker;
        }
        public SuggestionQueryResult Map(IEnumerable<GeoDataEntry> results, SuggestionQuery suggestionQuery, ILinkBuilder linkBuilder)
        {

            if (linkBuilder == null)
            {
                throw new ArgumentNullException(nameof(linkBuilder));
            }

            IEnumerable<Suggestion> suggestions = null;

            if (results != null)
            {


                //if suggestionQuery is empty, there's nothing to rank against
                if (suggestionQuery == null || suggestionQuery.IsEmpty())
                {
                    suggestions = results.Select(r => new Suggestion()
                    {
                        Certainty = 0,
                        Longitude = r.Longitude,
                        Latitude = r.Latitude,
                        Name = r.Name
                    }).OrderBy(r => r.Name);
                }
                else
                {
                    Coordinates coordinates = string.IsNullOrEmpty(suggestionQuery.Longitude)
                   ? null
                   : Coordinates.FromString(suggestionQuery.Latitude, suggestionQuery.Longitude);


                    suggestions = CertaintyRanker.Rank(results, suggestionQuery.Q, coordinates)
                    .Select(c => new Suggestion()
                    {
                        Certainty = c.Certainty,
                        Longitude = c.Entry.Longitude,
                        Latitude = c.Entry.Latitude,
                        Name = c.Entry.Name,
                        Links = linkBuilder.BuildLinks(c.Entry)
                    }).OrderByDescending(r => r.Certainty);
                }


            }

            //if suggestions are null, return an empty array
            return new SuggestionQueryResult() {Suggestions = suggestions ?? new Suggestion[] {}};

        }

       
    }
}