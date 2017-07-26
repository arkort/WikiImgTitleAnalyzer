using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiImgTitleAnalyzer.Core.Entities.Exceptions
{
    /// <summary>
    /// Exception class for similarity checks
    /// </summary>
    public class SimilarityCheckException : Exception
    {
        private const string MESSAGE = "Error occured during similarity check. Please see inner exception for details.";

        public SimilarityCheckException(Exception innerException) 
            : base("", innerException)
        {
        }
    }
}
