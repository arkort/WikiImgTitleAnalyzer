using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiImgTitleAnalyzer.Core.Entities
{
    /// <summary>
    /// Internal class that contains string and all of its symbol pairs
    /// </summary>
    class ComparisonString
    {
        private const string WHITESPACE = " ";

        /// <summary>
        /// Initial string
        /// </summary>
        public string StringValue { get; private set; }

        /// <summary>
        /// Set of pairs
        /// </summary>
        public HashSet<string> SymbolPairs { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="str">Initial string</param>
        public ComparisonString(string str)
        {
            StringValue = str;

            SymbolPairs = new HashSet<string>();

            for (int i = 0; i < str.Length - 1; i++)
            {
                var symPair = str.Substring(i, 2);

                if (!symPair.Contains(WHITESPACE))
                {
                    SymbolPairs.Add(symPair.ToLowerInvariant());
                }
            }
        }
    }
}
