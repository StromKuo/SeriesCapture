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

            try
            {
                var urls = await GetEpisodeUrls(seriesUrl);
                foreach (var episodeUri in urls)
                {
                    ret.Add(await this.GetMagnetLink(episodeUri));
                }
            }
            catch (HtmlWebException e)
            {
                Console.WriteLine($"{e.Message}");
            }

            return ret;
        }

        public async Task<List<Uri>> GetEpisodeUrls(string seriesUrl)
        {
            var ret = new List<Uri>();

            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(seriesUrl);

            if (htmlDoc == null)
            {
                throw new HtmlWebException($"Null html document loaded, url: {seriesUrl}");
            }
            else if (htmlDoc.DocumentNode == null)
            {
                throw new HtmlWebException($"Null node in html document, url: {seriesUrl}");
            }

            var sss = htmlDoc.DocumentNode.SelectNodes("//*[@id='ajax_tbody']/tr/td[2]/a");

            if (sss == null)
            {
                throw new HtmlWebException($"Episode urls node list is null, url: {seriesUrl}");
            }

            foreach (var item in sss)
            {
                var relative = item.GetAttributeValue("href", string.Empty);
                ret.Add(new Uri(new Uri(seriesUrl), System.Web.HttpUtility.HtmlDecode(relative)));
            }

            return ret;
        }

        public async Task<string> GetMagnetLink(Uri uri)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = await web.LoadFromWebAsync(uri.AbsoluteUri);

            if (htmlDoc == null)
            {
                throw new HtmlWebException($"Null html document loaded, url: {uri.AbsoluteUri}");
            }
            else if (htmlDoc.DocumentNode == null)
            {
                throw new HtmlWebException($"Null node in html document, url: {uri.AbsoluteUri}");
            }

            var magnetLinkNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='torrent_url']");

            if (magnetLinkNode == null)
            {
                throw new HtmlWebException($"magnetLinkNode is null, url: {uri.AbsoluteUri}");
            }

            return magnetLinkNode.InnerText;
        }
    }
}
