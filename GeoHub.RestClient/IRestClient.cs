using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoHub.RestClient
{
    public  interface  IRestClient
    {
        IServiceResponse<T> Get<T>(string request) where T:new() ;
    }
}
