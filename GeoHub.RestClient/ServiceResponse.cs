namespace GeoHub.RestClient
{
    public class ServiceResponse<T> : IServiceResponse<T>
    {
      
        public T Data { get; set; }
        public string Content { get; set; }
    }
}