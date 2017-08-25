namespace GeoHub.RestClient
{
    public interface IClientFactory
    {
        IRestClient GetClient(string baseUrl);

    }
}