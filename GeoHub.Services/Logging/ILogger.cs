using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoHub.Services.Logging
{
    public interface IAppLogger
    {
        void Error(string message, Exception ex);
        void Info(string message);
    }
}