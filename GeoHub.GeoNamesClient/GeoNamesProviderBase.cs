using System;
using GeoHub.Logic;

namespace GeoHub.GeoNamesClient
{
    public abstract class GeoNamesProviderBase
    {
        protected const string userName = "jamesmckeon";
        protected string ApiUrlRoot
        {
            get { return "http://api.geonames.org"; }
        }

        /// <summary>
        /// returns GeoNames Api queystring parameters for a BoundingBox intstance
        /// </summary>
        /// <param name="boundingBox">required</param>
        /// <returns></returns>
        public static string GetCoordinateArgs(BoundingBox boundingBox)
        {
            if (boundingBox == null)
            {
                throw new ArgumentNullException(nameof(boundingBox));
            }

            //north=48.3124&south=45.2246&east=-120.8716&west=-123.0235
            var coordinates = BoundingBoxCoordinates.GetInKilometers(boundingBox.Point, boundingBox.Radius);
            return string.Format("north={0}&south={1}&east={2}&west={3}", coordinates.MaxPoint.Latitude,
                coordinates.MinPoint.Latitude, coordinates.MaxPoint.Longitude, coordinates.MinPoint.Longitude);
        }
    }
}