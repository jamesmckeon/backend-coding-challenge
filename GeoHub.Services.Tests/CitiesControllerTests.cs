using System.Web.Http.Results;
using GeoHub.Services.Controllers;
using GeoHub.Services.Data;
using NUnit.Framework;

namespace GeoHub.Services.Tests
{
    public class CitiesControllerTests : ControllerTestsBase
    {
        protected CitiesController Target { get; set; }

        [SetUp]
        public void SetUp()
        {
            base.TestSetUp();
            Target = new CitiesController(GeoDataProvider.Object, AppLogger.Object, ResultsMapper.Object);
        }

        //[Test]
        //public void Get_NullResults_ReturnsResult()
        //{
        //    const string testValue = "Test";
        //    var result = Target.SearchLargeCities(new SuggestionQuery() {Q = testValue});

        //    result.da
        //}

        [Test]
        public void SearchLargeCities_NullResults_ReturnsOk()
        {
            const string testValue = "Test";
            var result = Target.SearchLargeCities(new SuggestionQuery() {Q = testValue});

            Assert.IsInstanceOf<OkNegotiatedContentResult<SuggestionQueryResult>>(result);
        }

        
    }
}