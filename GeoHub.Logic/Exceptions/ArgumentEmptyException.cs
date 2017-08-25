using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoHub.Logic.Exceptions
{
    /// <summary>
    /// Thrown when a required parameter not null but empty
    /// </summary>
    public class ArgumentEmptyException:ArgumentException
    {
        public ArgumentEmptyException(string paramName): base("A required parameter is not null but empty.", paramName) { }
        public ArgumentEmptyException(string message, string paramName): base(message, paramName) { }
    }
}
