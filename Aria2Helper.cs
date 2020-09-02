using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

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

		public async void AddDownload(string url, string directory = null)
        {
            //         using (Stream stream = this.BuildJsonRequest(url, directory))
            //         {
            //             var res = this._client.PostAsync(this._host, new StreamContent(stream));
            //	Console.WriteLine(res);
            //}

            var json = this.BuildJsonRequest2(url, null);
            var res2 = await this._client.PostAsync(this._host, new StringContent(json));
            Console.WriteLine(res2);

            //var jsonRequest = BuildJsonRequest3(url, null);
            //using (var webClient = new WebClient())
            //{
            //    var response = webClient.UploadString(this._host, "POST", jsonRequest);
            //    Console.WriteLine(response);
            //}
        }

		Stream BuildJsonRequest(string uri, string directory = null)
		{
			var jsonObject = new JObject();
			jsonObject["jsonrpc"] = "2.0";
			jsonObject["id"] = "qwert";
			jsonObject["method"] = "aria2.addUri";
			jsonObject["token"] = this._token;

			var requestParams = new JArray();
			var uris = new JArray();
			uris.Add(uri);
			requestParams.Add(uris);
			if (!string.IsNullOrEmpty(directory))
			{
				var options = new JObject();
				options["dir"] = directory;
				requestParams.Add(options);
			}

			jsonObject["params"] = requestParams;
			//string json = JsonConvert.SerializeObject(jsonObject);

			var stream = new MemoryStream();
			using (StreamWriter sw = new StreamWriter(stream))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(sw, jsonObject);
			}
			
			//var json = jsonObject.ToString();
			return stream;
		}

		string BuildJsonRequest2(string uri, string directory = null)
		{
			var jsonObject = new JObject();
			jsonObject["jsonrpc"] = "2.0";
			jsonObject["id"] = "qwert";
			jsonObject["method"] = "aria2.addUri";
			//jsonObject["token"] = this._token;

			var requestParams = new JArray($"token:{this._token}");
			requestParams.Add(new JArray(uri));

			if (!string.IsNullOrEmpty(directory))
			{
				var options = new JObject();
				options["dir"] = directory;
				requestParams.Add(options);
			}

			jsonObject["params"] = requestParams;
			return JsonConvert.SerializeObject(jsonObject);
		}

		string BuildJsonRequest3(string uri, string name = null)
		{
			var jsonObject = new JObject();
			jsonObject["jsonrpc"] = "2.0";
			jsonObject["id"] = "qweryhtt";
			jsonObject["method"] = "aria2.addUri";
			var requestParams = new JArray($"token:{this._token}");
			var uris = new JArray();
			uris.Add(uri);
			requestParams.Add(uris);
			if (!string.IsNullOrEmpty(name))
			{
				var options = new JObject();
				options["dir"] = name;
				requestParams.Add(options);
			}
			jsonObject["params"] = requestParams;
			string json = JsonConvert.SerializeObject(jsonObject);
			return json;
		}
	}
}
