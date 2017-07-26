using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiImgTitleAnalyzer.Interfaces
{
    /// <summary>
    /// Base interface for string similarity processors
    /// </summary>
    public interface ISimilarityStringProcessor
    {
        /// <summary>
        /// Checks how similar two strings are
        /// </summary>
        /// <param name="stringOne">String one</param>
        /// <param name="stringTwo">String two</param>
        /// <returns>Double number between 0 and 1.</returns>
        double GetSimilarity(string stringOne, string stringTwo);

        IEnumerable<string> GetMostSimilar(IEnumerable<string> strings);

    }
}
