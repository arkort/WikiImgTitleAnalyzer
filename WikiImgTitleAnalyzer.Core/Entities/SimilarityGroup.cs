using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiImgTitleAnalyzer.Interfaces;

namespace WikiImgTitleAnalyzer.Core.Entities
{
    /// <summary>
    /// Class that aggregates strings that have same similarity index
    /// </summary>
    public class SymbolPairsSimilarityGroup : ISimilarityResult
    {
        /// <summary>
        /// Group index
        /// </summary>
        public int SimilarityIndex { get; set; }

        /// <summary>
        /// Strings that are similar according to Group index
        /// </summary>
        public HashSet<string> SimilarStrings { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SymbolPairsSimilarityGroup()
        {
            SimilarStrings = new HashSet<string>();
        }

        /// <summary>
        /// Add string to the group
        /// </summary>
        /// <param name="value">String to add</param>
        public void Add(string value)
        {
            SimilarStrings.Add(value);
        }
        
        /// <summary>
        /// Merge group to another one
        /// </summary>
        /// <param name="otherGroup">Group where current will be merged</param>
        public void Merge(SymbolPairsSimilarityGroup otherGroup)
        {
            if (GroupContainsAny(otherGroup))
            {
                foreach(var str in SimilarStrings)
                {
                    otherGroup.SimilarStrings.Add(str);
                }
            }
        }

        /// <summary>
        /// Checks if the group contains some string
        /// </summary>
        /// <param name="item">String to check</param>
        /// <returns>true is contains, false otherwise</returns>
        public bool Contains(string item)
        {
            return SimilarStrings.Contains(item);
        }

        private bool GroupContainsAny(SymbolPairsSimilarityGroup otherGroup)
        {
            foreach(var str in otherGroup.SimilarStrings)
            {
                if (SimilarStrings.Contains(str))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
