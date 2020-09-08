using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeriesCapture
{
    class Zhuixinfan
    {
        public async Task<List<string>> GetAllMagnetLinks(string seriesUrl)
        {
            var ret = new List<string>();

            var urls = GetEpisodeUrls(seriesUrl);
            foreach (var episodeUri in urls)
            {
                ret.Add(await this.GetMagnetLink(episodeUri));
            }

            return ret;
        }

        public List<Uri> GetEpisodeUrls(string seriesUrl)
        {
            var ret = new List<Uri>();

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(seriesUrl);
            var url = new Uri(seriesUrl);

            var sss = htmlDoc.DocumentNode.SelectNodes("//*[@id='ajax_tbody']/tr/td[2]/a");
            foreach (var item in sss)
            {
                var relative = item.GetAttributeValue("href", string.Empty);
                ret.Add(new Uri(url, System.Web.HttpUtility.HtmlDecode(relative)));
            }

            return ret;
        }

        public async Task<string> GetMagnetLink(Uri uri)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(uri.AbsoluteUri);

            var magnetLinkNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='torrent_url']");
            return magnetLinkNode.InnerText;
        }
    }
}
