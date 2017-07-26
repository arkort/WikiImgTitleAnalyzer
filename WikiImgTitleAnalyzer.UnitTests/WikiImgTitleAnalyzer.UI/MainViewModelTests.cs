using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WikiImgTitleAnalyzer.UI;
using Moq;
using WikiImgTitleAnalyzer.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace WikiImgTitleAnalyzer.UnitTests.WikiImgTitleAnalyzer.UI
{
    [TestClass]
    public class MainViewModelTests
    {
        private MainViewModel _vm;

        [TestInitialize]
        public void Prepare()
        {
            Mock<IHttpGateway> gatewayMock = new Mock<IHttpGateway>();
            gatewayMock
                .Setup(g => g.GetArticleIdsAsync(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<int>()))
                .Returns(
                    Task.FromResult<IEnumerable<int>>
                    (
                        new List<int>() { 1, 2, 3 }
                    )
                );

            gatewayMock
                .Setup(g => g.GetImageTitlesAsync(It.IsAny<int[]>()))
                .Returns(
                    Task.FromResult<IEnumerable<string>>
                    (
                        new List<string>() { "Similar1", "Similar2", "Not similar" }
                    )
                );

            Mock<ISimilarityStringProcessor> stringProcessorMock = new Mock<ISimilarityStringProcessor>();
            stringProcessorMock.Setup(g => g.GetMostSimilar(It.IsAny<IEnumerable<string>>())).Returns(
                (IEnumerable<string> init) => { return init.Where(x => x.StartsWith("Similar")); });

            _vm = new MainViewModel(gatewayMock.Object, stringProcessorMock.Object);
        }

        [TestMethod]
        public async Task StartExecuteTest()
        {
            _vm.Latitude = 10;
            _vm.Longtitude = 10;
            await _vm.StartExecute();
            Assert.AreEqual("Similar1\nSimilar2", _vm.SimilarStrings);
        }
    }
}
