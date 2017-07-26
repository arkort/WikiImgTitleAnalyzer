using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiImgTitleAnalyzer.Interfaces
{
    /// <summary>
    /// Interface for results of similarity checks
    /// </summary>
    public interface ISimilarityResult
    {
        /// <summary>
        /// Contains most similar string array
        /// </summary>
        HashSet<string> SimilarStrings { get; }

        /// <summary>
        /// Contains index of most similar strings (e.g. 1 for 10%, 2 for 20%, ...)
        /// </summary>
        int SimilarityIndex { get; }
    }
}
