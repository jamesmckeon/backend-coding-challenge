using System;
using System.Collections.Generic;
using System.Linq;
using GeoHub.Logic;
using GeoHub.RestClient;

namespace GeoHub.GeoNamesClient
{
    public class LargeCityDataProvider :GeoNamesProviderBase, IGeoDataProvider
    {

        //http://api.geonames.org/search?name_startsWith=Lon&north=54.85764&south=50.373496&east=17.6&west=-0.62&maxRows=20&username=jamesmckeon

        protected IClientFactory ClientFactory { get; set; }
        protected IGeoNamesEntryNameBuilder NameBuilder { get; set; }

        public LargeCityDataProvider(IClientFactory clientFactory):this(clientFactory, new GeoNamesCityNameBuilder()) { }
        
        public LargeCityDataProvider(IClientFactory clientFactory, IGeoNamesEntryNameBuilder cityNameBuilder)
        {
            if (clientFactory == null)
            {
                throw new ArgumentNullException(nameof(clientFactory));
            }

            ClientFactory = clientFactory;

            if (cityNameBuilder == null)
            {
                throw new ArgumentNullException(nameof(cityNameBuilder));
            }

            NameBuilder = cityNameBuilder;
        }

        public IQueryable<GeoDataEntry> Search(string searchTerm)

        {

            IQueryable<GeoDataEntry> results = null;

            var requestString =
                string.Format("searchJSON?name_startsWith={0}&userName=jamesmckeon&cities=cities15000&maxRows={1}",
                    searchTerm, 1000);


            var response = ClientFactory.GetClient(ApiUrlRoot).Get<GeoNamesResponse>(requestString);

            if (response != null)
            {
                results = response.Data?.Geonames?.Where(nm => nm.Population >= 300000 && nm.Population <= 1000000).Select(
                        result =>
                            new GeoDataEntry()
                            {
                                Name = NameBuilder.BuildName(result),
                                Latitude = Convert.ToDouble(result.Lat),
                                Longitude = Convert.ToDouble(result.Lng)

                            }).AsQueryable();
            }

            return results;
        }

        public IQueryable<GeoDataEntry> SearchNear(string searchTerm, BoundingBox boundingBox)
        {
            if (boundingBox == null)
            {
                throw new ArgumentNullException(nameof(boundingBox));
            }


            IQueryable<GeoDataEntry> results = null;
            string requestString = null;

            if (string.IsNullOrEmpty(searchTerm))
            {

                requestString = string.Format("searchJSON?username={0}&cities=cities15000&maxRows={1}&{2}", userName, maxRows, GetCoordinateArgs(boundingBox));
            }
            else
            {
                
                requestString = string.Format("searchJSON?name_startsWith={0}&username={1}&cities=cities15000&maxRows={2}&{3}", searchTerm, userName, maxRows, GetCoordinateArgs(boundingBox));
            }


            var response = ClientFactory.GetClient(ApiUrlRoot).Get<GeoNamesResponse>(requestString);

            if (response != null && response.Data != null && response.Data.Geonames != null)
            {
                results = response.Data.Geonames.Select(
                    result =>
                        new GeoDataEntry()
                        {
                            Name = NameBuilder.BuildName(result),
                            Latitude = Convert.ToDouble(result.Lat),
                            Longitude = Convert.ToDouble(result.Lng)

                        }).AsQueryable();
            }

            return results;
        }

      
    }
}