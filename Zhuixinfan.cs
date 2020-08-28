using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SeriesCapture
{
    class Zhuixinfan
    {
        public async Task<List<string>> GetEpisodeUrls(string seriesUrl)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(seriesUrl);

            var sss = htmlDoc.GetElementbyId("ajax_tbody").SelectNodes("//.td2");
            foreach (var item in sss)
            {
            }
            Console.WriteLine("a");
            return null;
        }
    }
}
