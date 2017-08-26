using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GeoHub.Services;
using GeoHub.Services.Logging;

namespace GeoHub.Services
{
    public class WebApiApplication : System.Web.HttpApplication
    {
     protected IAppLogger Logger = new NlogLogger();
        protected void Application_Start()
        {

            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();

        }

        protected void Application_Error(object sender, EventArgs e)
        {
        
            Exception ex = Server.GetLastError();
            Logger.Error("UnhandledExceptions", ex);
        }
    }
}
