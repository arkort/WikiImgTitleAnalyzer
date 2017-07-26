using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WikiImgTitleAnalyzer.Interfaces;

namespace WikiImgTitleAnalyzer.Core.Gateways
{
    /// <summary>
    /// Getter of articles and images from Wikipedia
    /// </summary>
    public class WikipediaGateway : IHttpGateway
    {
        #region Private fields

        private static readonly HttpClient _client = new HttpClient();

        private string _getArticlesUrl;
        private string _getImagesUrl;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="getArticlesUrl">URL template for getting articles</param>
        /// <param name="getImagesUrl">URL template for getting images</param>
        public WikipediaGateway(string getArticlesUrl, string getImagesUrl)
        {
            _getArticlesUrl = getArticlesUrl;
            _getImagesUrl = getImagesUrl;
        }

        #region Interface members

        /// <summary>
        /// Gets article ids for current position on map
        /// </summary>
        /// <param name="latitude">Latitude</param>
        /// <param name="longtitude">Longtitude</param>
        /// <param name="count">Article count</param>
        /// <returns>Array of ids</returns>
        public async Task<IEnumerable<int>> GetArticleIdsAsync(double latitude, double longtitude, int count)
        {
            var response = await GetHttpResponseJson(_getArticlesUrl, latitude, longtitude, count);

            return response.SelectTokens("query.geosearch[*].pageid").Select(t => (int)t);
        }

        /// <summary>
        /// Gets image titles from articles
        /// </summary>
        /// <param name="articleIds">Ids of requested articles</param>
        /// <returns>Array of titles</returns>
        public async Task<IEnumerable<string>> GetImageTitlesAsync(params int[] articleIds)
        {
            var response = await GetHttpResponseJson(_getImagesUrl, ConstructPageIdsArrayString(articleIds));

            return response.SelectTokens("query.pages.*.images[*].title").Select(t => (string)t);
        }

        #endregion

        #region Non-public members

        private async Task<JObject> GetHttpResponseJson(string baseUrl, params object[] parameters)
        {
            var response = await _client.GetAsync(ConstructFinalUrl(baseUrl, parameters));

            return (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
        }

        private string ConstructFinalUrl(string baseUrl, params object[] parameters)
        {
            return string.Format(baseUrl, parameters);
        }

        private string ConstructPageIdsArrayString(params int[] pageIds)
        {
            return string.Join("|", pageIds);
        }

        #endregion
    }
}
