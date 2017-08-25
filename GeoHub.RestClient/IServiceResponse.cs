namespace GeoHub.RestClient
{
    public interface IServiceResponse<T>
    {
        T Data { get; set; }
        string Content { get; set; }
    }
}