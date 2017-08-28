using System;
using System.Linq;
using GeoHub.Logic;
using GeoHub.RestClient;

namespace GeoHub.GeoNamesClient
{
    public class HospitalProvider : GeoNamesProviderBase, IGeoDataProvider
    {

        protected IClientFactory ClientFactory { get; set; }
        protected IGeoNamesEntryNameBuilder NameBuilder { get; set; }

        public HospitalProvider(IClientFactory clientFactory) : this(clientFactory, new GeoNamesCityNameBuilder()) { }

        public HospitalProvider(IClientFactory clientFactory, IGeoNamesEntryNameBuilder cityNameBuilder)
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

        public IQueryable<GeoDataEntry> Search(string searchTerm, int maxResults = 50)

        {

            //http://api.geonames.org/searchJSON?username=jamesmckeon&featureCode=CTRM

            if (string.IsNullOrEmpty(searchTerm))
            {
                throw new ArgumentNullException(nameof(maxResults));
            }


            IQueryable<GeoDataEntry> results = null;


            var requestString = string.Format("searchJSON?name_startsWith={0}&username={1}&featureCode=HSP&maxRows={2}", searchTerm, userName, maxResults);

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

        public IQueryable<GeoDataEntry> SearchNear(string searchTerm, BoundingBox boundingBox, int maxResults = 50)
        {
            if (boundingBox == null)
            {
                throw new ArgumentNullException(nameof(boundingBox));
            }


            IQueryable<GeoDataEntry> results = null;
            string requestString = null;

            if (string.IsNullOrEmpty(searchTerm))
            {
                requestString = string.Format("searchJSON?username={0}&featureCode=HSP&featureCode=CTRM&maxRows={1}&{2}", userName, maxResults, GetCoordinateArgs(boundingBox));
            }
            else
            {
                requestString = string.Format("searchJSON?name_startsWith={0}&username={1}&featureCode=HSP&featureCode=CTRM&maxRows={2}&{3}", searchTerm, userName, maxResults, GetCoordinateArgs(boundingBox));
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