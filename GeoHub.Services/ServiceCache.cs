using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoHub.Logic;
using GeoHub.Services.Logging;

namespace GeoHub.Services
{


    /// <summary>
    /// Provides thread-safe caching of service data
    /// </summary>
    /// <remarks>Refreshes cache if duration has expired, or if the last search for a searchTerm/Latitude/Longitude combination
    /// was executed for a smaller maxResults value than is current</remarks>
    public class ServiceCache : IGeoDataProvider
    {
        /// <summary>
        /// Uniquely identifies an IGeoDataProvider query
        /// </summary>
        public class ServiceCacheKey : IEquatable<ServiceCacheKey>
        {

            private DateTime _createdDate;
            private string _searchTerm = null;
            private BoundingBox _boundingBox = null;
            private int _maxResults = -1;

            public DateTime CreatedTime
            {
                get { return _createdDate; }
            }

            /// <summary>
            /// The value of maxResults that was passed to the underlying IGeoDataProvider instance when it was last queried
            /// </summary>
            public int MaxResults
            {
                get { return _maxResults; }
            }
            public string SearchTerm
            {
                get { return _searchTerm; }
            }

            public BoundingBox BoundingBox
            {
                get { return _boundingBox; }
            }
            protected IEqualityComparer<string> StringComparer { get; set; }

            /// <param name="stringComparer"></param>
            /// <param name="boundingBox"></param>
            /// <param name="searchTerm"></param>
            /// <param name="maxResults"></param>
            public ServiceCacheKey(IEqualityComparer<string> stringComparer, BoundingBox boundingBox, string searchTerm, int maxResults)
            {
                if (stringComparer == null)
                {
                    throw new ArgumentNullException(nameof(stringComparer));
                }

                StringComparer = stringComparer;

                if (string.IsNullOrEmpty(searchTerm) && boundingBox == null)
                {
                    throw new ArgumentException("Either searchTerm or boundingBox must be provided");
                }

                if (maxResults < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(maxResults));
                }

                _boundingBox = boundingBox;
                _searchTerm = searchTerm;
                _maxResults = maxResults;
                _createdDate = DateTime.Now;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as ServiceCacheKey);
            }

            public override int GetHashCode()
            {
                if (BoundingBox == null)
                {
                    return StringComparer.GetHashCode(SearchTerm);
                }
                else
                {
                    return StringComparer.GetHashCode(SearchTerm) + BoundingBox.GetHashCode();
                }
            }

            public bool Equals(ServiceCacheKey other)
            {
                if (other == null)
                {
                    return false;
                }
                else if (BoundingBox == null)
                {
                    return StringComparer.Equals(SearchTerm, other.SearchTerm);
                }
                else
                {
                    return StringComparer.Equals(SearchTerm, other.SearchTerm) && BoundingBox.Equals(other.BoundingBox);
                }
            }
        }
        protected TimeSpan CacheDuration { get; set; }
        protected IEqualityComparer<string> StringComparer { get; set; }
        protected Dictionary<ServiceCacheKey, IQueryable<GeoDataEntry>> Entries { get; set; }
        protected IGeoDataProvider DataProvider { get; set; }
        protected IAppLogger Logger { get; set; }

        /// <summary>
        /// The  minimum maxResults value that will be passed to  IGeoDataProvider instances
        /// </summary>
        protected int DataProviderMinResults { get; set; }
        /// <summary>
        /// Instantiates a ServiceCache
        /// </summary>
        /// <param name="cacheDuration">The cache expiration timespan (i.e., how long data should be cached for) (required)</param>
        /// <param name="geoDataProvider">The IGeoDataProvder instance whose data is being cached (required)</param>
         /// <param name="logger">(required)</param>
        /// <param name="stringComparer">(required)</param>
        /// <param name="minDataProviderResults">The minimum maxResults value that will be passed to this cache's IGeoDatProvider</param>
        public ServiceCache(TimeSpan cacheDuration, IGeoDataProvider geoDataProvider, IAppLogger logger, IEqualityComparer<string> stringComparer, int minDataProviderResults = 50)
        {

            if (geoDataProvider == null)
            {
                throw new ArgumentNullException(nameof(geoDataProvider));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (stringComparer == null)
            {
                throw new ArgumentNullException(nameof(stringComparer));
            }

            DataProvider = geoDataProvider;
            CacheDuration = cacheDuration;
            StringComparer = stringComparer;
            Entries = new Dictionary<ServiceCacheKey, IQueryable<GeoDataEntry>>();
            Logger = logger;
            DataProviderMinResults = minDataProviderResults;
        }


        public IQueryable<GeoDataEntry> Search(string searchTerm, int maxResults)
        {
            return Search(searchTerm, null, maxResults);
        }

        protected IQueryable<GeoDataEntry> Search(string searchTerm, BoundingBox boundingBox, int maxResults)
        {
           

            lock (Entries)
            {
                //always want to query at least DataProviderMinResults # of entries  so that 
                //Ranking logic is (most) accurately applied
                maxResults = Math.Max(maxResults, DataProviderMinResults);

                //remove entry if it already exists and is expired
                var newKey = new ServiceCacheKey(StringComparer, boundingBox, searchTerm, maxResults);
                ServiceCacheKey existingKey = Entries.ContainsKey(newKey) ? Entries.Keys.Single(k => k.Equals(newKey)) : null;

                if (existingKey != null && IsFlushable(existingKey))
                {
                    Entries.Remove(existingKey);
                    existingKey = null;
                }


                //if this set of args hasn't been queried before, or
                //if it has but for a lower number of maxResults, refresh cache for this key
                if (existingKey == null || existingKey.MaxResults < maxResults)
                {
                    if (boundingBox != null)
                    {
                        Entries[newKey] = DataProvider.SearchNear(searchTerm, boundingBox, maxResults);
                    }
                    else
                    {
                        Entries[newKey] = DataProvider.Search(searchTerm.Trim(), maxResults);
                    }

                }
                return Entries[newKey];
            }


        }

        public IQueryable<GeoDataEntry> SearchNear(string searchTerm, BoundingBox boundingBox, int maxResults)
        {
            return Search(searchTerm, boundingBox, maxResults);

        }

        /// <summary>
        /// returns true if a cache entry has expired
        /// </summary>
        /// <returns></returns>
        protected bool IsFlushable(ServiceCacheKey key)
        {
            return DateTime.Now - key.CreatedTime > CacheDuration;
        }

    }
}