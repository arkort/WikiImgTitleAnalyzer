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
    public class SymbolPairsSimilarityProcessorTests
    {
        SymbolPairsSimilarityProcessor _processor = SymbolPairsSimilarityProcessor.Instance;

        [TestMethod]
        public void GetSimilarityTest()
        {
            var similarity = _processor.GetSimilarity("MARTHA", "MARHTA");
            Assert.AreEqual(similarity, 0.4);

            similarity = _processor.GetSimilarity("FRANCE", "FRENCH");
            Assert.AreEqual(similarity, 0.4);

            similarity = _processor.GetSimilarity("a", "FRENCH");
            Assert.AreEqual(similarity, 0);
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
            CollectionAssert.AreEquivalent(new[] { "Iteration", "Another iteration" }, similar.SimilarStrings.ToArray());
        }
    }
}
