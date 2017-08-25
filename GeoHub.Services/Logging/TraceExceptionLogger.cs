using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace GeoHub.Services.Logging
{
    public class TraceExceptionLogger : ExceptionLogger
    {
        protected IAppLogger Logger { get; set; }

        public TraceExceptionLogger():this(new NlogLogger()) { }
        public TraceExceptionLogger(IAppLogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            Logger = logger;
        }
        public override void Log(ExceptionLoggerContext context)
        {
            Logger.Error("Unhandled exception", context.ExceptionContext.Exception);
           
        }
    }
}