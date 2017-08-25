using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoHub.Services
{
    /// <summary>
    /// Specifies values used for attribute routing
    /// </summary>
    /// <remarks>In theory attribute routing should only be used when the app needs to expose a method that doesn't match a controller name</remarks>
    public class Routes
    {
        public const string Suggestions = "api/suggestions";
    }
}