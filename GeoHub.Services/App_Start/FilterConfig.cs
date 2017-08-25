using System.Web;
using System.Web.Mvc;
using GeoHub.Services.Filters;

namespace GeoHub.Services
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
         
        }
    }
}
