using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SeriesCapture
{
    class Zhuixinfan
    {
        public List<string> GetAllMagnetLinks(string seriesUrl)
        {
            var ret = new List<string>();

            var urls = GetEpisodeUrls(seriesUrl);
            foreach (var episodeUri in urls)
            {
                ret.Add(this.GetMagnetLink(episodeUri));
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

        public string GetMagnetLink(Uri url)
        {
            HtmlWeb web = new HtmlWeb();
            var tmp = url.AbsoluteUri;// "http://www.zhuixinfan.com/main.php?mod=viewresource&sid=11562";
            var htmlDoc = web.Load(tmp);

            //var a = htmlDoc.GetElementbyId("torrent_url");
            //var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id='torrent_url']");
            //throw new Exception();
            var magnetLinkNode = htmlDoc.DocumentNode.SelectSingleNode("//*[@id='torrent_url']");
            var a = magnetLinkNode.InnerHtml;
            var b = magnetLinkNode.GetDirectInnerText();
            var c = magnetLinkNode.ToString();
            var d = htmlDoc.ParsedText;
            return magnetLinkNode.InnerText;
        }
    }
}
