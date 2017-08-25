using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GeoHub.Logic;
using GeoHub.Services.Data;
using GeoHub.Services.Exceptions;
using GeoHub.Services.Logging;

namespace GeoHub.Services.Controllers
{
    public class HospitalsController : ControllerBase
    {

        public HospitalsController(IGeoDataProvider geoDataProvider, IAppLogger appLogger,
           IResultsMapper resultsMapper) : base(geoDataProvider, appLogger, resultsMapper) { }

        public IHttpActionResult Get([FromUri]SuggestionQuery query)
        {

            Coordinates sourceCoordinates = null;

            if (!string.IsNullOrEmpty(query.Latitude) || !string.IsNullOrEmpty(query.Longitude))
            {
                try
                {
                    sourceCoordinates = Coordinates.FromString(query.Latitude, query.Longitude);
                }
                catch (ArgumentException ex)
                {

                    throw new ParameterException(ex.ParamName);
                }

            }

            IEnumerable<GeoDataEntry> results = null;
            try
            {
                if (sourceCoordinates == null)
                {
                    results = DataProvider.Search(query?.Q);
                }
                else
                {
                    results = DataProvider.SearchNear(query?.Q,
                        new BoundingBox(sourceCoordinates, Defaults.defaultRadiusKm));
                }

            }
            catch (Exception ex)
            {
                AppLogger.Error("error querying hospitals", ex);
                throw new DataProviderException(ex);
            }

            return Ok(ResultsMapper.Map(results, query, new LinkBuilder(this)));

        }


    }
}
