using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoHub.Services.Data
{
    public class SuggestionQueryResult
    {
        public IEnumerable<Suggestion> Suggestions { get; set; }
    }
}