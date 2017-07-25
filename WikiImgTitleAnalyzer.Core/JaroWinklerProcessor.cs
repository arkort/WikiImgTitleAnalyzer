using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiImgTitleAnalyzer.Interfaces;

namespace WikiImgTitleAnalyzer.Core
{
    /// <summary>
    /// Class that uses Jaro-Winkler algorithm for string similarity checks
    /// </summary>
    public class JaroWinklerProcessor : ISimilarityStringProcessor
    {
        private const double P = 0.1;
        private const int MAX_PREFIX_LENGTH = 4;

        private static readonly JaroWinklerProcessor _instance = new JaroWinklerProcessor();

        /// <summary>
        /// Gets the instance of JaroWinklerProcessor
        /// </summary>
        public static JaroWinklerProcessor Instance
        {
            get => _instance;
        }

        private JaroWinklerProcessor()
        {

        }

        /// <summary>
        /// Checks how similar two strings are
        /// </summary>
        /// <param name="stringOne">String one</param>
        /// <param name="stringTwo">String two</param>
        /// <param name="includeCase">If true then method will consider one symbol in different cases as different symbols</param>
        /// <returns>Double number between 0 and 1.</returns>
        public double GetSimilarity(string stringOne, string stringTwo, bool includeCase = false)
        {
            var str1 = includeCase ? stringOne : stringOne.ToLowerInvariant();
            var str2 = includeCase ? stringTwo : stringTwo.ToLowerInvariant();

            int m = 0; //Count of same symbols
            int t = 0; //Count of symbols on other places

            var deviation = (int)Math.Floor(Math.Max(str1.Length, str2.Length) / 2.0) - 1;

            for (int i = 0; i < str1.Length; i++)
            {
                if (str1.GetSymbolByIndexSafe(i) == str2.GetSymbolByIndexSafe(i))
                {
                    m++;
                    continue;
                }

                if (stringTwo.IsSymbolInSubstringRange(stringOne[i], i - deviation, i + deviation))
                {
                    m++;
                    t++;
                }
            }

            if (m == 0)
            {
                return 0;
            }
            else
            {
                double mDouble = m;
                var jaroValue = 1.0 / 3 * ((mDouble / str1.Length) + (mDouble / str2.Length) + ((mDouble - t / 2.0) / mDouble));

                var l = Math.Min(str1.GetCommonPrefixLength(str2), MAX_PREFIX_LENGTH);
                return (jaroValue + l * P * (1 - jaroValue));
            }
        }

        public IEnumerable<string> GetMostSimilar(IEnumerable<string> strings, bool includeCase = false)
        {
            var groups = new List<SimilarityGroup>();
            var stringsList = strings.ToList();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            Parallel.For(0, stringsList.Count - 1, (i) =>
            {
                Dictionary<int, SimilarityGroup> similarityGroupsInner = new Dictionary<int, SimilarityGroup>();

                for (int j = i + 1; j < stringsList.Count; j++)
                {
                    if (stringsList[i] == stringsList[j])
                    {
                        continue;
                    }

                    var similarity = GetSimilarity(stringsList[i], stringsList[j], includeCase);

                    var key = (int)(similarity * 10);

                    if (!similarityGroupsInner.ContainsKey(key))
                    {
                        similarityGroupsInner.Add(key, new SimilarityGroup() { GroupIndex = key });
                    }

                    if (!similarityGroupsInner[key].Contains(stringsList[i]))
                    {
                        similarityGroupsInner[key].Add(stringsList[i]);
                    }
                    similarityGroupsInner[key].Add(stringsList[j]);
                }

                var maxKeyItem = similarityGroupsInner.OrderByDescending(x => x.Key).First();
                groups.Add(maxKeyItem.Value);
            });


            var maxIndex = groups.OrderByDescending(x => x.GroupIndex).First().GroupIndex;
            watch.Stop();
            var maxGroups = groups
                .Where(x => x.GroupIndex == maxIndex).ToList();

            for(int i = 0; i < maxGroups.Count() - 1; i++)
            {
                for(int j = i + 1; j < maxGroups.Count(); j++)
                {
                    maxGroups[i].Merge(maxGroups[j]);
                }
            }

            return maxGroups.OrderByDescending(x => x.Strings.Count).First().Strings;
        }
    }
}
