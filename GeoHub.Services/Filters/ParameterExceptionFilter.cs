using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using GeoHub.Services.Exceptions;

namespace GeoHub.Services.Filters
{
    public class ParameterExceptionFilter:ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ParameterException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
               
            }
        }
    }
}
