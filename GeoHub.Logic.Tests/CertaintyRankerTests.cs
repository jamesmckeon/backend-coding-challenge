using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GeoHub.Logic;
using Moq;
namespace GeoHub.Logic.Tests
{
    [TestFixture]
    public class CertaintyRankerTests
    {
        protected CertaintyRanker Target { get; set; }

        [SetUp]
        public void SetUp()
        {
            Target = new CertaintyRanker();
        }

        [Test]
        public void Rank_TermAndCoordinatesNull_ThrowsExpected()
        {
            var ex =
                Assert.Throws<ArgumentException>(
                    () => Target.Rank(Enumerable.Range(1, 5).Select(i => new GeoDataEntry()), null, null));

            Assert.AreEqual(CertaintyRanker.EitherSourcecoordinatesOrSearchtermMustBeProvidedMessage, ex.Message);
        }

        [Test]
        public void Rank_CoordinatesNullPerfectTermMatch_ReturnsExpectedCertainty()
        {

            //verify that whitespaces are trimmed and certainty is calculated accurately
            //entries contains a Name that matches searchTerm exactly, so max(Certainty) for matching term should == 100%
            var entries = new Tuple<GeoDataEntry, double>[]
            {
                new Tuple<GeoDataEntry, double>(new GeoDataEntry() {Name = "Test"}, 1.0), //full match
                new Tuple<GeoDataEntry, double>(new GeoDataEntry() {Name = "Test 2 "}, .6666), //whitespaces should be trimmed
                new Tuple<GeoDataEntry, double>(new GeoDataEntry() {Name = "Test 3 xx"}, .4444),
                new Tuple<GeoDataEntry, double>(new GeoDataEntry() {Name = "Something"}, 0) //doesn't start with searchTerm

           };


            foreach (RankingResult result in Target.Rank(entries.Select(e => e.Item1), "test", null))
            {
                Assert.AreEqual(entries.Where(e => e.Item1.Equals(result.Entry)).Single().Item2, result.Certainty, .0001);
            }

        }

        [Test]
        public void Rank_CoordinatesNullPartialTermMatch_ReturnsExpectedCertainty()
        {


            //vverify that in the case of only partial matches, results are still offset/normalized to 100%
            var entries = new Tuple<GeoDataEntry, double>[]
            {
                new Tuple<GeoDataEntry, double>(new GeoDataEntry() {Name = "Test"}, 1.0), //full match
                new Tuple<GeoDataEntry, double>(new GeoDataEntry() {Name = "Test 2 "}, .6666), //whitespaces should be trimmed
                new Tuple<GeoDataEntry, double>(new GeoDataEntry() {Name = "Test 3 xx"}, .4444),
                new Tuple<GeoDataEntry, double>(new GeoDataEntry() {Name = "Something"}, 0) //doesn't start with searchTerm

           };


            foreach (RankingResult result in Target.Rank(entries.Select(e => e.Item1), "te", null))
            {
                Assert.AreEqual(entries.Where(e => e.Item1.Equals(result.Entry)).Single().Item2, result.Certainty, .0001);
            }

        }

        [Test]
        public void Rank_ByDistance_ReturnsExpectedCertainty()
        {
            var newYork = Coordinates.NewYork();
          
            
            var entries = new GeoDataEntry[]
            {

                new GeoDataEntry() {Name = "Portland",  Latitude = 45.512794, Longitude = -122.679565}, //2448.63  miles away
               
                 new GeoDataEntry() {Name = "Chicago", Latitude = 41.881832, Longitude = -87.623177    }, //715.91
        
                 new GeoDataEntry() {Name = "Paris", Latitude = 48.864716, Longitude = 2.349014    }, //3632.78 
  
                 new GeoDataEntry() {Name = "Buenos Aires", Latitude = -34.603722, Longitude =-58.381592    }
            };


            var minDistance = entries.Select(r => newYork.DistanceFrom(r.Latitude, r.Longitude)).Min();
       
            foreach (RankingResult result in Target.Rank(entries, null, newYork))
            {
                var entry = entries.Where(r => r.Equals(result.Entry)).Single();
                Assert.AreEqual(minDistance/newYork.DistanceFrom(entry), result.Certainty, .0001);
            }

        }

        [Test]
        public void Rank_ByDistance_HasExactMatch_ReturnsExpectedCertainty()
        {
            var newYork = Coordinates.NewYork();


            var entries = new GeoDataEntry[]
            {
                new GeoDataEntry() {Name = "NYC",  Latitude = newYork.Latitude, Longitude = newYork.Longitude},
                new GeoDataEntry() {Name = "Portland",  Latitude = 45.512794, Longitude = -122.679565},
               
                 new GeoDataEntry() {Name = "Chicago", Latitude = 41.881832, Longitude = -87.623177    }, 
        
                 new GeoDataEntry() {Name = "Paris", Latitude = 48.864716, Longitude = 2.349014    },
  
                 new GeoDataEntry() {Name = "Buenos Aires", Latitude = -34.603722, Longitude =-58.381592    }
            };


            foreach (RankingResult result in Target.Rank(entries, null, newYork))
            {
                var expectedCertainty = result.Entry.Name.Equals("NYC") ? 1.0 : 0;
                
                Assert.AreEqual(expectedCertainty, result.Certainty);
            }



        }

        [Test]
        public void Rank_ByDistance_HasExactMatches_ReturnsExpectedCertainty()
        {
            var newYork = Coordinates.NewYork();


            var entries = new GeoDataEntry[]
            {
                new GeoDataEntry() {Name = "NYC",  Latitude = newYork.Latitude, Longitude = newYork.Longitude},
                new GeoDataEntry() {Name = "A hotdog stand in NYC",  Latitude = newYork.Latitude, Longitude = newYork.Longitude},
                new GeoDataEntry() {Name = "Portland",  Latitude = 45.512794, Longitude = -122.679565},

                 new GeoDataEntry() {Name = "Chicago", Latitude = 41.881832, Longitude = -87.623177    },

                 new GeoDataEntry() {Name = "Paris", Latitude = 48.864716, Longitude = 2.349014    },

                 new GeoDataEntry() {Name = "Buenos Aires", Latitude = -34.603722, Longitude =-58.381592    }
            };


            foreach (RankingResult result in Target.Rank(entries, null, newYork))
            {
                var expectedCertainty = result.Entry.Latitude == newYork.Latitude && result.Entry.Longitude == newYork.Longitude ? 0.5 : 0;

                Assert.AreEqual(expectedCertainty, result.Certainty);
            }

        }

    }
}
