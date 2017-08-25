using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoHub.RestClient
{
    public class RestClientFactory:IClientFactory
    {

        protected static IClientFactory factory = null;

        protected  IList<RestSharpClient> Clients { get; private set; }

        RestClientFactory()
        {
            Clients = new List<RestSharpClient>();
        }

        public static IClientFactory Instance
        {
            get
            {
                if (factory == null)
                {
                    factory = new RestClientFactory();
                }
                return factory;
            }
        }


        public IRestClient GetClient(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            lock (Clients)
            {
                if (!Clients.Any(c => c.BaseUrl.Equals(baseUrl)))
                {
                    Clients.Add(new RestSharpClient(baseUrl));
                }
            }

            return Clients.Where(c => c.BaseUrl.Equals(baseUrl)).Single();
        }
    }
}
