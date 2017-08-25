using System;
using System.Web.Http;
using GeoHub.Logic;
using GeoHub.Services.Logging;


namespace GeoHub.Services.Controllers
{
    public abstract class ControllerBase:ApiController
    {
       
        protected IGeoDataProvider DataProvider { get; set; }
        protected IAppLogger AppLogger { get; set; }
        protected  IResultsMapper ResultsMapper { get; set; }

        protected ControllerBase(IGeoDataProvider dataProvider, IAppLogger appLogger,IResultsMapper resultsMapper)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProvider));
            }

            DataProvider = dataProvider;

            if (appLogger == null)
            {
                throw new ArgumentNullException(nameof(appLogger));
            }

            AppLogger = appLogger;

            if (resultsMapper == null)
            {
                throw new ArgumentNullException(nameof(resultsMapper));
            }

            ResultsMapper = resultsMapper;
        }

    }
}