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
        public List<string> Strings { get; set; }

        public SimilarityGroup()
        {
            Strings = new List<string>();
        }

        public void Add(string value)
        {
            Strings.Add(value);
        }

        public void Merge(SimilarityGroup otherGroup)
        {
            if (GroupContainsAny(otherGroup))
            {
                Strings.AddRange(otherGroup.Strings);
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
