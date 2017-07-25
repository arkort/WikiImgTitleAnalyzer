using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiImgTitleAnalyzer.Core;

namespace WikiImgTitleAnalyzer.UnitTests.WikiImgTitleAnalyzer.Core
{
    [TestClass]
    public class JaroWinklerProcessorTests
    {
        JaroWinklerProcessor _processor = JaroWinklerProcessor.Instance;

        [TestMethod]
        public void GetSimilarityTest()
        {
            var similarity = _processor.GetSimilarity("MARTHA", "MARHTA");
            Assert.AreEqual(0.96, Math.Round(similarity, 2));
        }

        [TestMethod]
        public void GetMostSimilarTest()
        {
            var stringList = new List<string>()
            {
                "Item",
                "Some more item",
                "Iteration",
                "Another iteration",
                "Actual date",
                "Data0",
                "Rationality",
                "Random string",
                "Integrity"
            };

            var similar = _processor.GetMostSimilar(stringList);
        }
    }
}
