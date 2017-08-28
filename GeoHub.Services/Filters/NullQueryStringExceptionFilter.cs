using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using GeoHub.Services.Exceptions;

namespace GeoHub.Services.Filters
{
    public class NullQueryStringExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NullQueryStringException) ;
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}