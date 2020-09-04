using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SeriesCapture
{
    class Program
    {
        const string DefaultConfigPath = "./config.json";
        const uint IntervalMinutes = 60;

        /// <summary>
        /// Series url and its download links
        /// </summary>
        static readonly Dictionary<string, HashSet<string>> _snapshot = new Dictionary<string, HashSet<string>>();

        static readonly Zhuixinfan _zhuixinfan = new Zhuixinfan();

        static Aria2Helper _aria2Helper = null;

        static async Task Main(string[] args)
        {
            //args = new string[] { @"C:\Users\Strom\Documents\Repositories\NET\SeriesCapture\config.json" };
            var configPath = ReadConfigPathFromArgs(args);
            configPath = string.IsNullOrEmpty(configPath) ? DefaultConfigPath : configPath;

            var configData = ReadData(configPath);

            if (configData != null)
            {
                _aria2Helper = new Aria2Helper(configData.aria2Config.host, configData.aria2Config.token);

                _snapshot.Clear();

                Console.WriteLine("Taking a snapshot of all series start.");
                foreach (var seriesData in configData.seriesDatas)
                {
                    _snapshot.Add(seriesData.seriesUrl, new HashSet<string>(await _zhuixinfan.GetAllMagnetLinks(seriesData.seriesUrl)));
                }
                Console.WriteLine("Taking a snapshot of all series complete.");
            }

            while (true)
            {
                Console.WriteLine($"Schedule to grab series at {DateTime.Now + TimeSpan.FromMinutes(IntervalMinutes)}");
                await Task.Delay(TimeSpan.FromMinutes(IntervalMinutes));

                foreach (var seriesData in configData.seriesDatas)
                {
                    var currentLinks = await _zhuixinfan.GetAllMagnetLinks(seriesData.seriesUrl);
                    var dirtyLinks = GetDirtyLinks(_snapshot[seriesData.seriesUrl], currentLinks);

                    if (dirtyLinks.Count > 0)
                    {
                        Console.WriteLine("Series update found!");

                        foreach (var dlink in dirtyLinks)
                        {
                            await _aria2Helper.AddDownload(dlink, seriesData.directory);
                            Console.WriteLine($"Added download task to {seriesData.directory}");
                            _snapshot[seriesData.seriesUrl].Add(dlink);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No series update found.");
                    }
                }
            }
        }

        static string ReadConfigPathFromArgs(string[] args)
        {
            if (args.Length >= 1 && !string.IsNullOrEmpty(args[0]))
            {
                return args[0];
            }
            return null;
        }

        static Config ReadData(string path)
        {
            Console.WriteLine($"Reading config from {path}");
            if (File.Exists(path))
            {
                string fileContents;
                using (StreamReader sr = File.OpenText(path))
                {
                    fileContents = sr.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<Config>(fileContents);
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
