using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoHub.Logic;
using Moq;
using NUnit.Framework;
using GeoHub.Services;
using GeoHub.Services.Logging;

namespace GeoHub.Services.Tests
{
    [TestFixture]
    public class ServiceCacheTests
    {
        protected class ConstructedTestCache
        {
            public  Mock<IGeoDataProvider> DataProvider { get; set; }
            public Mock<IAppLogger> AppLoger { get; set; }
            public Mock<IEqualityComparer<string>> StringComparer { get; set; }
            public TimeSpan CacheDuration { get; set; }
            public int MinDataProviderResults { get; set; }
            public ServiceCache TestCache { get; set; } 

        }

        protected ConstructedTestCache GetTestCache(TimeSpan cacheDuration)
        {
            var ret = new ConstructedTestCache()
            {
                DataProvider = new Mock<IGeoDataProvider>(),
                AppLoger = new Mock<IAppLogger>(),
                CacheDuration = cacheDuration,
                MinDataProviderResults = 50,
                StringComparer = new Mock<IEqualityComparer<string>>()
            };

            ret.TestCache = new ServiceCache(ret.CacheDuration, ret.DataProvider.Object, ret.AppLoger.Object, ret.StringComparer.Object, ret.MinDataProviderResults );

            return ret;
        }

        [Test]
        public void Search_CacheExpired_RefreshesEntry()
        {
            //verify that ServiceCache pings its wrapped DataProvider twice
            //because the cache expiration is zero

            var cache = GetTestCache(TimeSpan.MinValue);
            const string testSearchTerm = "someterm";
        
            cache.DataProvider.Setup(
                s => s.Search(It.Is<string>(str => str.Trim().Equals(testSearchTerm.Trim())), cache.MinDataProviderResults))
                .Returns(
                Enumerable.Range(1, 10).Select(i => new GeoDataEntry()).AsQueryable()
                );

            //not testing here that correct string is being passed around
            cache.StringComparer.Setup(s => s.Equals(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            //call method twice, verify data provider gets called twice
            cache.TestCache.Search(testSearchTerm, 10);
            cache.TestCache.Search(testSearchTerm, 10);

            cache.DataProvider.Verify(s => s.Search(testSearchTerm.Trim(), cache.MinDataProviderResults), Times.Exactly(2));
        }

        [Test]
        public void Search_maxResultsDifferent_RefreshesEntry()
        {
            //verify that ServiceCache rereshes an entry if the 
            //value of maxResults is different than the last time it was called


            //prevent refreshing due to cache expiration
            var cache = GetTestCache(TimeSpan.MaxValue);
            const string testSearchTerm = "someterm";

            cache.DataProvider.Setup(
                s => s.Search(It.Is<string>(str => str.Trim().Equals(testSearchTerm.Trim())), It.IsAny<int>()))
                .Returns(
                Enumerable.Range(1, 10).Select(i => new GeoDataEntry()).AsQueryable()
                );

            //not testing here that correct string is being passed around
            cache.StringComparer.Setup(s => s.Equals(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            //call method twice, verify data provider gets called twice
            cache.TestCache.Search(testSearchTerm, 10);

            //cache should only refresh if maxResults exceeds the 
            //MinDataProviderResults it was instantiated with
            cache.TestCache.Search(testSearchTerm, cache.MinDataProviderResults+1);

            cache.DataProvider.Verify(s => s.Search(testSearchTerm.Trim(), cache.MinDataProviderResults), Times.Once);
            cache.DataProvider.Verify(s => s.Search(testSearchTerm.Trim(), cache.MinDataProviderResults+1), Times.Once);
        }

        [Test]
        public void Search_maxResultsNotExceeded_DoesntRefreshEntry()
        {
            //verify that ServiceCache doesnt ping GeoDataProvider an entry if the 
            //value of maxResults is less than or equal to the last time it was called


            //prevent refreshing due to cache expiration
            var cache = GetTestCache(TimeSpan.MaxValue);
            const string testSearchTerm = "someterm";

            cache.DataProvider.Setup(
                s => s.Search(It.Is<string>(str => str.Trim().Equals(testSearchTerm.Trim())), It.IsAny<int>()))
                .Returns(
                Enumerable.Range(1, 10).Select(i => new GeoDataEntry()).AsQueryable()
                );

            //not testing here that correct string is being passed around
            cache.StringComparer.Setup(s => s.Equals(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            //call method twice, verify data provider gets called twice
            cache.TestCache.Search(testSearchTerm, 10);

            //cache should only refresh if maxResults exceeds the 
            //MinDataProviderResults it was instantiated with
            cache.TestCache.Search(testSearchTerm, cache.MinDataProviderResults);
            cache.TestCache.Search(testSearchTerm, cache.MinDataProviderResults -1);

            cache.DataProvider.Verify(s => s.Search(testSearchTerm.Trim(), cache.MinDataProviderResults), Times.Once);
        }

        [Test]
        public void Search_CacheNotExpired_DoesntRefreshEntry()
        {
            //verify that ServiceCache pings its wrapped DataProvider only once
            //because the cached entry hasn't expired

            var cache = GetTestCache(TimeSpan.MaxValue);
            const string testSearchTerm = "someterm";

            cache.DataProvider.Setup(
                s => s.Search(It.Is<string>(str => str.Trim().Equals(testSearchTerm.Trim())), cache.MinDataProviderResults))
                .Returns(
                Enumerable.Range(1, 10).Select(i => new GeoDataEntry()).AsQueryable()
                );

            //not testing here that correct string is being passed around
            cache.StringComparer.Setup(s => s.Equals(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            //call method twice, verify data provider gets called twice
            cache.TestCache.Search(testSearchTerm, 10);
            cache.TestCache.Search(testSearchTerm, 10);

            cache.DataProvider.Verify(s => s.Search(testSearchTerm.Trim(), cache.MinDataProviderResults), Times.AtMostOnce);
        }

    }
}
