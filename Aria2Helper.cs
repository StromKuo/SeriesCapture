using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SeriesCapture
{
    class Aria2Helper
    {
		readonly HttpClient _client = new HttpClient();

		string _host = "http://localhost:6800/jsonrpc";
		string _token;

		public Aria2Helper(string host, string token)
        {
			this._host = host;
			this._token = token;
		}

		public async Task AddDownload(string url, string directory = null)
        {
            using (Stream stream = this.BuildJsonRequest(url, directory))
			{
				var res = await this._client.PostAsync(this._host, new StreamContent(stream));
				Console.WriteLine(res);
			}
        }

		Stream BuildJsonRequest(string uri, string directory = null)
		{
			var jsonObject = new JObject();
			jsonObject["jsonrpc"] = "2.0";
			jsonObject["id"] = "qwert";
			jsonObject["method"] = "aria2.addUri";

			var requestParams = new JArray($"token:{this._token}");
			requestParams.Add(new JArray(uri));

			if (!string.IsNullOrEmpty(directory))
			{
				var options = new JObject();
				options["dir"] = directory;
				requestParams.Add(options);
			}

			jsonObject["params"] = requestParams;

            var stream = new MemoryStream();
            using (var streamWriter = new StreamWriter(stream: stream, leaveOpen: true))
			using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jsonWriter, jsonObject);
				jsonWriter.Flush();
				streamWriter.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
		}
	}
}
