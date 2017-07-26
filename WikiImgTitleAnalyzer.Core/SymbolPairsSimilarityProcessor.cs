using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WikiImgTitleAnalyzer.Core.Entities;
using WikiImgTitleAnalyzer.Interfaces;

namespace WikiImgTitleAnalyzer.Core
{
    /// <summary>
    /// Class that checks string similarity by analyzing common symbol pairs
    /// </summary>
    public class SymbolPairsSimilarityProcessor : ISimilarityStringProcessor
    {
        private const double P = 0.1;
        private const int MAX_PREFIX_LENGTH = 4;

        private static readonly SymbolPairsSimilarityProcessor _instance = new SymbolPairsSimilarityProcessor();

        /// <summary>
        /// Gets the instance of JaroWinklerProcessor
        /// </summary>
        public static SymbolPairsSimilarityProcessor Instance
        {
            get => _instance;
        }

        private SymbolPairsSimilarityProcessor()
        {

        }

        /// <summary>
        /// Internal method that processes string comparison
        /// </summary>
        /// <param name="stringOne"></param>
        /// <param name="stringTwo"></param>
        /// <returns></returns>
        double GetSimilarity(ComparisonString stringOne, ComparisonString stringTwo)
        {
            var intersection = stringOne.SymbolPairs.Intersect(stringTwo.SymbolPairs);
            return 2.0 * intersection.Count() / (stringOne.SymbolPairs.Count + stringTwo.SymbolPairs.Count);
        }

        /// <summary>
        /// Checks how similar two strings are
        /// </summary>
        /// <param name="stringOne">String one</param>
        /// <param name="stringTwo">String two</param>
        /// <param name="includeCase">If true then method will consider one symbol in different cases as different symbols</param>
        /// <returns>Double number between 0 and 1.</returns>
        public double GetSimilarity(string stringOne, string stringTwo)
        {
            return GetSimilarity(new ComparisonString(stringOne), new ComparisonString(stringTwo));
        }

        /// <summary>
        /// Gets most similar strings
        /// </summary>
        /// <param name="strings">Initial list of strings</param>
        /// <returns>Set of most similar strings</returns>
        public IEnumerable<string> GetMostSimilar(IEnumerable<string> strings)
        {
            var groups = new List<SimilarityGroup>();
            var stringsList = strings.ToList();

            // Find most similar strings for each string in array
            Parallel.For(0, stringsList.Count - 1, (i) =>
            {
                GetSimilarityGroupsFromParticularString(i, stringsList, groups);
            });

            // As we need MOST similar strings - use max group index
            var maxIndex = groups.OrderByDescending(x => x.GroupIndex).First().GroupIndex;

            // Get all the groups with maximum index (here will be groups of different i strings)
            var maxGroups = groups.Where(x => x.GroupIndex == maxIndex).ToList();

            // Merge groups if necessary
            ProcessGroupMerging(maxGroups);

            return maxGroups.OrderByDescending(x => x.Strings.Count).First().Strings;
        }

        /// <summary>
        /// Compares i string to all the others and gets some similarity groups
        /// </summary>
        /// <param name="i"></param>
        /// <param name="stringsList"></param>
        /// <param name="groups"></param>
        private void GetSimilarityGroupsFromParticularString(int i, List<string> stringsList, List<SimilarityGroup> groups)
        {
            Dictionary<int, SimilarityGroup> similarityGroupsInner = new Dictionary<int, SimilarityGroup>();

            for (int j = i + 1; j < stringsList.Count; j++)
            {
                // Equal strings are not processed
                if (stringsList[i] == stringsList[j])
                {
                    continue;
                }

                // Get similarity value for i and j strings
                var similarity = GetSimilarity(stringsList[i], stringsList[j]);

                // Create a group index - this value will be used to generate similarity groups, so strings with the same index go to the same group
                var groupIndex = (int)(similarity * 10);

                if (!similarityGroupsInner.ContainsKey(groupIndex))
                {
                    similarityGroupsInner.Add(groupIndex, new SimilarityGroup() { GroupIndex = groupIndex }); // Add new group if necessary
                }

                similarityGroupsInner[groupIndex].Add(stringsList[j]);
            }

            // For current i string get the group with maximum index (these are most similar strings) and add it to global similarity groups
            var maxIndexGroup = similarityGroupsInner.OrderByDescending(x => x.Key).First();
            groups.Add(maxIndexGroup.Value);
        }

        /// <summary>
        /// This method checks if there are any intersections between different groups. If there are it just merges them and puts the result instead of the j group. This way we get the largest similar string group.
        /// </summary>
        /// <param name="maxGroups"></param>
        private void ProcessGroupMerging(List<SimilarityGroup> maxGroups) 
        {
            for (int i = 0; i < maxGroups.Count() - 1; i++)
            {
                for (int j = i + 1; j < maxGroups.Count(); j++)
                {
                    maxGroups[i].Merge(maxGroups[j]);
                }
            }
        }
    }
}
