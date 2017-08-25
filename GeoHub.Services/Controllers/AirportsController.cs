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

    public class AirportsController : ControllerBase
    {

        public AirportsController(IGeoDataProvider geoDataProvider, IAppLogger appLogger,
           IResultsMapper resultsMapper) : base(geoDataProvider, appLogger, resultsMapper) { }


        public IHttpActionResult Get([FromUri]SuggestionQuery query)
        {

            Coordinates sourceCoordinates = null;

            if (query != null)
            {
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
            }

          

            IEnumerable<GeoDataEntry> results = null;

            try
            {
                string searchTerm = query?.Q;

                if (sourceCoordinates == null)
                {
                    results = DataProvider.Search(searchTerm);
                }
                else
                {
                    results = DataProvider.SearchNear(searchTerm,
                        new BoundingBox(sourceCoordinates, Defaults.defaultRadiusKm));
                }

            }
            catch (Exception ex)
            {
                AppLogger.Error("error querying airports", ex);
                throw new DataProviderException(ex);
            }

            return Ok(ResultsMapper.Map(results, query, new LinkBuilder(this)));

        }


    }
}
