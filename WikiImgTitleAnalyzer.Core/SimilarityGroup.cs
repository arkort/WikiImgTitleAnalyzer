using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiImgTitleAnalyzer.Core
{
    /// <summary>
    /// Class that aggregates strings that have same similarity index
    /// </summary>
    class SimilarityGroup
    {
        /// <summary>
        /// Group index
        /// </summary>
        public int GroupIndex { get; set; }

        /// <summary>
        /// Strings that are similar according to Group index
        /// </summary>
        public HashSet<string> Strings { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public SimilarityGroup()
        {
            Strings = new HashSet<string>();
        }

        /// <summary>
        /// Add string to the group
        /// </summary>
        /// <param name="value">String to add</param>
        public void Add(string value)
        {
            Strings.Add(value);
        }
        
        /// <summary>
        /// Merge group to another one
        /// </summary>
        /// <param name="otherGroup">Group where current will be merged</param>
        public void Merge(SimilarityGroup otherGroup)
        {
            if (GroupContainsAny(otherGroup))
            {
                foreach(var str in Strings)
                {
                    otherGroup.Strings.Add(str);
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
            return Strings.Contains(item);
        }

        private bool GroupContainsAny(SimilarityGroup otherGroup)
        {
            foreach(var str in otherGroup.Strings)
            {
                if (Strings.Contains(str))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
