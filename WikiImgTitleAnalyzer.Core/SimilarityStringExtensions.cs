using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiImgTitleAnalyzer.Interfaces;

namespace WikiImgTitleAnalyzer.Core
{
    public static class SimilarityStringExtensions
    {
        private static ISimilarityStringProcessor _processor = JaroWinklerProcessor.Instance;

        /// <summary>
        /// Get a symbol that stands on 'index' position in a current string. If index is out of range then '\0' will be returned.
        /// </summary>
        /// <param name="str">Initial string</param>
        /// <param name="index">Symbol index</param>
        /// <returns>Requested symbol or \0</returns>
        public static string GetSymbolByIndexSafe(this string str, int index)
        {
            var result = str.ElementAtOrDefault<char>(index);
            return result != default(char) ? result.ToString() : null;
        }

        /// <summary>
        /// Gets a substring. Method does not throw exceptions when trying to process indexes that are out of range.
        /// </summary>
        /// <param name="str">A string to process</param>
        /// <param name="startIndex">A start index of substring</param>
        /// <param name="length">Length of substring</param>
        /// <returns>A substring</returns>
        public static string SubstringSafe(this string str, int startIndex, int length)
        {
            int startFixed;
            int lengthFixed;

            if (startIndex < 0)
            {
                startFixed = 0;
                lengthFixed = length + startIndex;
            }
            else
            {
                startFixed = startIndex;
                lengthFixed = length;
            }
            var endIndex = startFixed + lengthFixed;
            lengthFixed = endIndex < str.Length ? lengthFixed : str.Length - startFixed;

            return str.Substring(startFixed, lengthFixed);
        }

        /// <summary>
        /// Checks if there's a symbol in the string on positions between startIndex and endIndex
        /// </summary>
        /// <param name="str">String to check</param>
        /// <param name="symbol">Symbol to find</param>
        /// <param name="startIndex">The first index of position range</param>
        /// <param name="endIndex">The second index of position range</param>
        /// <returns>True is there is a symbol between indexes in a string</returns>
        public static bool IsSymbolInSubstringRange(this string str, char symbol, int startIndex, int endIndex)
        {
            return str.SubstringSafe(startIndex, endIndex - startIndex).Contains(symbol);
        }

        /// <summary>
        /// Get length of common prefix for two strings
        /// </summary>
        /// <param name="str">First string</param>
        /// <param name="otherString">Second string</param>
        /// <returns>Length of maximum common prefix</returns>
        public static int GetCommonPrefixLength(this string str, string otherString)
        {
            int prefixLength = 0;

            int index = 0;

            while (index < Math.Min(str.Length, otherString.Length) && str[index] == otherString[index])
            {
                prefixLength++;
                index++;
            }

            return prefixLength;
        }
    }
}
