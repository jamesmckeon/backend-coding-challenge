using System;
using System.Net;

namespace GeoHub.RestClient
{
    public class ClientExecutionException : WebException
    {
        public ClientExecutionException(Exception innerException):base("An error occurred during a service client operation.", innerException) { }

    }
}