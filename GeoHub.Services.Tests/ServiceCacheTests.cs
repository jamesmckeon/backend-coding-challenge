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

        [Test]
        public void Search_CacheExpired_PingsDataProviderTwice()
        {
            //verify that ServiceCache pings its wrapped DataProvider twice
            //because the cache expiration is zero
            //ServiceCache should provide trimmed input string to DataProvider without casing modified
            //implementing case-insensitive search is up to the DataProvider to implement at its discretion


            var logger = new Mock<IAppLogger>();
            var dataProvider = new Mock<IGeoDataProvider>();
       
            var cache = new ServiceCache(new TimeSpan(0), dataProvider.Object, logger.Object, new Logic.StringComparer());//timespan = 0, cache should always expire
            const string testSearchTerm = " sOme tErm ";
        
            dataProvider.Setup(
                s => s.Search(It.Is<string>(str => str.Trim().Equals(testSearchTerm.Trim()))))
                .Returns(
                Enumerable.Range(1, 10).Select(i => new GeoDataEntry()).AsQueryable()
                );

            //call method twice, verify data provider gets called twice
            cache.Search(testSearchTerm);
            cache.Search(testSearchTerm);

            dataProvider.Verify(s => s.Search(testSearchTerm.Trim()), Times.Exactly(2));
        }

        [Test]
        public void Search_CacheNotExpired_PingsDataProviderOnce()
        {

            //verify that ServiceCache pings its wrapped DataProvider once
            //because the cache expiration hasn't been exceeded
            //ServiceCache should provide trimmed input string to DataProvider without casing modified
            //implementing case-insensitive search is up to the DataProvider to implement at its discretion

            var logger = new Mock<IAppLogger>();
            var dataProvider = new Mock<IGeoDataProvider>();
            var cache = new ServiceCache(new TimeSpan(1,0,0,0), dataProvider.Object, logger.Object, new Logic.StringComparer());//timespan = 1 day, cache shouldn't expire during this method
            const string testSearchTerm = " sOme tErm ";

            dataProvider.Setup(
                s => s.Search(It.Is<string>(str => str.Trim().Equals(testSearchTerm.Trim()))))
                .Returns(
                Enumerable.Range(1, 10).Select(i => new GeoDataEntry()).AsQueryable()
                );

            //call method twice, verify data provider gets called twice
            cache.Search(testSearchTerm);
            cache.Search(testSearchTerm);

            dataProvider.Verify(s => s.Search(testSearchTerm.Trim()), Times.Exactly(1));
        }
    }
}
