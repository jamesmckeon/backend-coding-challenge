using System;
using RestSharp;

namespace GeoHub.RestClient
{
   
    public class RestSharpClient : IRestClient, IEquatable<RestSharpClient>
    {
        public string BaseUrl { get; private set; }
        

        protected  RestSharp.RestClient Client { get; set; }
        

        public RestSharpClient(string baseUrl) 
        {

            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            BaseUrl = baseUrl;
            Client = new RestSharp.RestClient(baseUrl);
        }
         
        public IServiceResponse<T> Get<T>(string request) where T:new() 
        {
            var restRequest = new RestRequest(request, Method.GET);

            try
            {
                var response = Client.Execute<T>(restRequest);

                if (response != null)
                {
                    return new ServiceResponse<T>() { Content = response.Content, Data = response.Data };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                
                throw new ClientExecutionException(ex);
            }

        }

        public bool Equals(RestSharpClient other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                return this.BaseUrl.Equals(other.BaseUrl);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RestSharpClient);
        }

        public override int GetHashCode()
        {
            return BaseUrl.GetHashCode();
        }
    }
}