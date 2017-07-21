using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiImgTitleAnalyzer.Interfaces
{
    public interface IHttpGateway
    {
        Task<IEnumerable<int>> GetArticleIdsAsync(double latitude, double longtitude, int count);
        Task<IEnumerable<string>> GetImageTitlesAsync(params int[] articleIds);

    }
}
