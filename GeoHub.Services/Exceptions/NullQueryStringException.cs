using System.Web;

namespace GeoHub.Services.Exceptions
{
    public class NullQueryStringException : HttpException
    {
        public NullQueryStringException():base("Zero query string parameters were provided were at least one is expected") { }
    }
}