using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.Unity;
using System.Web.Http;
using GeoHub.GeoNamesClient;
using Unity.WebApi;
using GeoHub.Logic;
using GeoHub.RestClient;
using GeoHub.Services.Controllers;
using GeoHub.Services.Logging;

namespace GeoHub.Services
{
    public static class UnityConfig
    {

        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            /*
            Add your Unity registrations in the RegisterComponents method of the UnityConfig class. All components that implement IDisposable should be
            registered with the HierarchicalLifetimeManager to ensure that they are properly disposed at the end of the request.*/

            var largeCityServiceCache = new ServiceCache(new TimeSpan(0, 4, 0, 0), new LargeCityDataProvider(RestClientFactory.Instance), new NlogLogger(), new Logic.StringComparer());
            var hospitalCache = new ServiceCache(new TimeSpan(0, 4, 0, 0), new HospitalProvider(RestClientFactory.Instance), new NlogLogger(), new Logic.StringComparer());
            var airportCache = new ServiceCache(new TimeSpan(0, 4, 0, 0), new AirportProvider(RestClientFactory.Instance), new NlogLogger(), new Logic.StringComparer());

            container.RegisterInstance<IGeoDataProvider>("largeCityProvider", largeCityServiceCache);
            container.RegisterInstance<IGeoDataProvider>("hospitalProvider", hospitalCache);
            container.RegisterInstance<IGeoDataProvider>("airportProvider", airportCache);

            container.RegisterInstance<IClientFactory>(RestClientFactory.Instance);

            container.RegisterType<CitiesController>(new InjectionConstructor(new ResolvedParameter<IGeoDataProvider>("largeCityProvider"), new NlogLogger(), new ResultsMapper(new CertaintyRanker()) ));
            container.RegisterType<HospitalsController>(new InjectionConstructor(new ResolvedParameter<IGeoDataProvider>("hospitalProvider"), new NlogLogger(), new ResultsMapper(new CertaintyRanker())));
            container.RegisterType<AirportsController>(new InjectionConstructor(new ResolvedParameter<IGeoDataProvider>("airportProvider"), new NlogLogger(), new ResultsMapper(new CertaintyRanker())));
            container.RegisterType<IAppLogger, NlogLogger>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);


        }
    }
}