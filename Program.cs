using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SeriesCapture
{
    class Program
    {
        const string TestUrl = "http://www.zhuixinfan.com/viewtvplay-1193.html";
        const string TestConfigPath = @"C:\Users\Strom\Documents\Repositories\NET\SeriesCapture\config.json";

        /// <summary>
        /// Series url and its download links
        /// </summary>
        static readonly Dictionary<string, HashSet<string>> _snapshot = new Dictionary<string, HashSet<string>>();

        static readonly Zhuixinfan _zhuixinfan = new Zhuixinfan();

        static async Task Main(string[] args)
        {
            _snapshot.Clear();

            var configData = ReadData();
            foreach (var seriesData in configData)
            {
                _snapshot.Add(seriesData.seriesUrl, new HashSet<string>(await _zhuixinfan.GetAllMagnetLinks(TestUrl)));
            }
        }

        static async Task TestAria2()
        {
            var dlink = "magnet:?xt=urn:btih:04051b54ed1343686cbabd256045906274b94a62&tr=http://tr.cili001.com:8070/announce&tr=udp://p4p.arenabg.com:1337&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://open.demonii.com:1337";
            var a = "https://aria2.frp.strom.cf:443/jsonrpc";
            var b = "http://localhost:6800/jsonrpc";
            var token = "qE!4&u0JV$1S";
            var aria2Helper = new Aria2Helper(b, token);
            var dir = @"D:\Downloads\半泽直树测试";

            await aria2Helper.AddDownload(dlink, dir);
        }

        static List<SeriesData> ReadData(string path = TestConfigPath)
        {
            if (File.Exists(path))
            {
                string fileContents;
                using (StreamReader sr = File.OpenText(path))
                {
                    fileContents = sr.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<List<SeriesData>>(fileContents);
            }
            else
            {
                Console.WriteLine("Config file not found");
                return null;
            }
        }

        static List<string> GetDirtyLinks(HashSet<string> oldLinks, List<string> newLinks)
        {
            var ret = new List<string>();

            foreach (var newLink in newLinks)
            {
                if (!oldLinks.Contains(newLink))
                {
                    ret.Add(newLink);
                }
            }

            return ret;
        }
    }
}
