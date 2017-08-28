using GeoHub.Logic;
using GeoHub.Services.Logging;
using Moq;
using NUnit.Framework;

namespace GeoHub.Services.Tests
{
    [TestFixture]
    public abstract class ControllerTestsBase
    {
        protected  Mock<IGeoDataProvider> GeoDataProvider { get; set; }
        protected  Mock<IAppLogger> AppLogger { get; set; }
        protected  Mock<IResultsMapper> ResultsMapper { get; set; }
      
        public void TestSetUp()
        {
            GeoDataProvider = new Mock<IGeoDataProvider>();
            AppLogger = new Mock<IAppLogger>();
            ResultsMapper = new Mock<IResultsMapper>();
       
        }

      
    }
}