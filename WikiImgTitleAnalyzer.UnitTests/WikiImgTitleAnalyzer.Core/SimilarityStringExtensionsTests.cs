using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WikiImgTitleAnalyzer.Core;

namespace WikiImgTitleAnalyzer.UnitTests.WikiImgTitleAnalyzer.Core
{
    [TestClass]
    public class SimilarityStringExtensionsTests
    {
        [TestMethod]
        public void SubstringSafeTest()
        {
            var str = "This is a sample string";

            Assert.AreEqual(str.SubstringSafe(5, 2), "is");
            Assert.AreEqual(str.SubstringSafe(-5, 6), "T");
            Assert.AreEqual(str.SubstringSafe(15, 20), "e string");

        }

        [TestMethod]
        public void IsSymbolInSubstringRangeTest()
        {
            var str = "This is a sample string";

            Assert.IsTrue(str.IsSymbolInSubstringRange('i', -1, 5));
            Assert.IsTrue(str.IsSymbolInSubstringRange('g', 15, 29));
            Assert.IsFalse(str.IsSymbolInSubstringRange('e', -1, 5));
        }

        [TestMethod]
        public void GetCommonPrefixLengthTest()
        {
            var string1 = "String1";
            var string2 = "String2";

            Assert.AreEqual(string1.GetCommonPrefixLength(string2), 6);

            var string3 = "Otther";
            Assert.AreEqual(string1.GetCommonPrefixLength(string3), 0);

        }

        //[TestMethod]
        //public void GetSimilarityTest()
        //{
        //    Assert.AreEqual(1,"string".GetSimilarity("string"));
        //    Assert.AreEqual(0, "string".GetSimilarity("foma"));
        //    Assert.AreEqual(0.96, Math.Round("martha".GetSimilarity("marhta")), 2);
        //}
    }
}
