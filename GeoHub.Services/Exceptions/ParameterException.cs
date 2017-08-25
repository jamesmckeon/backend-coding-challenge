using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoHub.Services.Exceptions
{
    public class ParameterException:Exception
    {
        public ParameterException(): base("A required parameter wasn't provided.") { }

        public ParameterException(string paramName) : base( string.Format("Parameter '{0}' is required", paramName) ){ }
    }
}