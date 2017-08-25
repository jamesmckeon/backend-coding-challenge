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
    public class ServiceCache : IGeoDataProvider
    {

        public class ServiceCacheKey : IEquatable<ServiceCacheKey>
        {


            private string _searchTerm = null;
            private BoundingBox _boundingBox = null;

            public string SearchTerm
            {
                get { return _searchTerm; }
            }

            public BoundingBox BoundingBox
            {
                get { return _boundingBox; }
            }
            protected IEqualityComparer<string> StringComparer { get; set; }
            public ServiceCacheKey(IEqualityComparer<string> stringComparer, BoundingBox boundingBox, string searchTerm)
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

                _boundingBox = boundingBox;
                _searchTerm = searchTerm;
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
        protected DateTime LastFlushed { get; set; }
        /// <summary>
        /// All entries that have been searched by key
        /// </summary>
        protected Dictionary<ServiceCacheKey, IQueryable<GeoDataEntry>> QueriedEntries { get; set; }
        /// <summary>
        /// All entries that have been searched without one or more search parameters provided
        /// </summary>
        protected List<GeoDataEntry> AllEntries { get; set; }
        protected IGeoDataProvider DataProvider { get; set; }
        protected IAppLogger Logger { get; set; }
        /// <summary>
        /// Instantiates a ServiceCache
        /// </summary>
        /// <param name="cacheDuration">The cache expiration timespan (i.e., how long data should be cached for) (required)</param>
        /// <param name="geoDataProvider">The IGeoDataProvder instance whose data is being cached (required)</param>
        ///    /// <param name="logger">(required)</param>
        /// <param name="stringComparer">(required)</param>
        public ServiceCache(TimeSpan cacheDuration, IGeoDataProvider geoDataProvider, IAppLogger logger, IEqualityComparer<string> stringComparer)
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
            LastFlushed = DateTime.Now;
            QueriedEntries = new Dictionary<ServiceCacheKey, IQueryable<GeoDataEntry>>();
            AllEntries = new List<GeoDataEntry>();
            Logger = logger;

        }

        private IQueryable<GeoDataEntry> GetAll()
        {
            lock (AllEntries)
            {
                if (AllEntries.Count == 0)
                {
                    AllEntries.AddRange(DataProvider.Search(null));
                }
            }

            return AllEntries.AsQueryable();
        }
        public IQueryable<GeoDataEntry> Search(string searchTerm)
        {
            return Search(searchTerm, null);
        }

        protected IQueryable<GeoDataEntry> Search(string searchTerm, BoundingBox boundingBox)
        {
            //flush cache if it has expired
            if (IsFlushable())
            {
                Flush();
            }


            //if searchTerm hasn't been provided, just return all entries
            if (string.IsNullOrEmpty(searchTerm) && boundingBox == null)
            {
                //TODO might perform better to returns AllEntries first and then flush cache if it's time to
                return GetAll();

            }
            else //return entries matching search term
            {
                lock (QueriedEntries)
                {
                    var key = new ServiceCacheKey(StringComparer, boundingBox, searchTerm);
                    if (!QueriedEntries.ContainsKey(key))
                    {
                        if (boundingBox != null)
                        {
                            QueriedEntries[key] = DataProvider.SearchNear(searchTerm, boundingBox);
                        }
                        else
                        {
                            QueriedEntries[key] = DataProvider.Search(searchTerm.Trim());
                        }
                        
                    }
                    return QueriedEntries[key];
                }

            }
        }

        public IQueryable<GeoDataEntry> SearchNear(string searchTerm, BoundingBox boundingBox)
        {
            return Search(searchTerm, boundingBox);

        }

        /// <summary>
        /// returns true if the cache duration has expired/been exceeded
        /// </summary>
        /// <returns></returns>
        protected bool IsFlushable()
        {
            return DateTime.Now - LastFlushed > CacheDuration;
        }

        protected void Flush()
        {
            lock (AllEntries)
            {
                AllEntries.Clear();
            }

            lock (QueriedEntries)
            {
                QueriedEntries.Clear();
            }

            LastFlushed = DateTime.Now;
        }
    }
}