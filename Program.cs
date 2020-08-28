using System;

namespace SeriesCapture
{
    class Program
    {
        const string testUrl = "http://www.zhuixinfan.com/viewtvplay-1193.html";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            new Zhuixinfan().GetEpisodeUrls(testUrl);
        }
    }
}
