using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiImgTitleAnalyzer.Interfaces
{
    /// <summary>
    /// Interface for getting image titles and articles through http
    /// </summary>
    public interface IHttpGateway
    {
        /// <summary>
        /// Gets article ids for current position on map
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longtitude">Longtitude</param>
        /// <param name="count">Article count</param>
        /// <returns>Array of ids</returns>
        Task<IEnumerable<int>> GetArticleIdsAsync(double latitude, double longtitude, int count);

        /// <summary>
        /// Gets image titles from articles
        /// </summary>
        /// <param name="articleIds">Ids of requested articles</param>
        /// <returns>Array of titles</returns>
        Task<IEnumerable<string>> GetImageTitlesAsync(params int[] articleIds);

    }
}
