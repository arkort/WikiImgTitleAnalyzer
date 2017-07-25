using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiImgTitleAnalyzer.Core
{
    class SimilarityGroup
    {
        public int GroupIndex { get; set; }
        public HashSet<string> Strings { get; set; }

        public SimilarityGroup()
        {
            Strings = new HashSet<string>();
        }

        public void Add(string value)
        {
            Strings.Add(value);
        }

        public void Merge(SimilarityGroup otherGroup)
        {
            if (GroupContainsAny(otherGroup))
            {
                foreach(var group in Strings)
                {
                    otherGroup.Strings.Add(group);
                }
                //Strings.AddRange(otherGroup.Strings);
            }
        }

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
