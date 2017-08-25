using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
namespace GeoHub.Services.Logging
{
    public class NlogLogger: IAppLogger
    {
        public void Error(string message, Exception ex)
        {
           LogManager.GetCurrentClassLogger().Error(ex, message);
        }

        public void Info(string message)
        {
           LogManager.GetCurrentClassLogger().Info(message);
        }
    }
}