using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WikiImgTitleAnalyzer.Core.Gateways;
using System.Threading.Tasks;
using System.Linq;
using WikiImgTitleAnalyzer.Interfaces;

namespace WikiImgTitleAnalyzer.IntegrationTests.WikiImgTitleAnalyzer.Core.Gateways
{
    [TestClass]
    public class WikipediaGatewayTests
    {
        IHttpGateway _gateway = new WikipediaGateway(
            @"https://en.wikipedia.org/w/api.php?action=query&list=geosearch&gsradius=10000&gscoord={0}|{1}&gslimit=50&format=json",
            @"https://en.wikipedia.org/w/api.php?action=query&prop=images&pageids={0}&format=json&imlimit=500");

        [TestMethod]
        public async Task GetArticleIdsTest()
        {
            var ids = await _gateway.GetArticleIdsAsync(37.786971, -122.399677, 50);

            Assert.IsNotNull(ids);
            Assert.AreEqual(ids.Count(), 50);
        }

        [TestMethod]
        public async Task GetImageTitlesTest()
        {
            var imageTitles = await _gateway.GetImageTitlesAsync(9292891, 18618509);

            Assert.IsNotNull(imageTitles);
            Assert.AreEqual(imageTitles.Count(), 10);
        }

        [TestMethod]
        public async Task GetImageTitlesForCustomArticlesTest()
        {
            var ids = await _gateway.GetArticleIdsAsync(37.786971, -122.399677, 50);
            var imageTitles = await _gateway.GetImageTitlesAsync(ids.ToArray());

            Assert.IsNotNull(imageTitles);
            Assert.AreEqual(imageTitles.Count(), 10);
        }
    }
}
