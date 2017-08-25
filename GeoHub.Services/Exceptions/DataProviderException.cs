using System;

namespace GeoHub.Services.Exceptions
{
    /// <summary>
    /// Represents an error incurred during an IDataProvider method execution
    /// </summary>
    public class DataProviderException : Exception
    {
        public DataProviderException(Exception innerException):base("An error occurred while executing an IDataProvider call", innerException) { }
    }
}