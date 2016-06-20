using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TwtichChatClient.Model
{
    public class HttpApiRequester : IApiRequester
    {
        private readonly HttpClient _httpClient;
        private Lazy<Task<dynamic>> _mainObj;
        public HttpApiRequester()
        {
            _httpClient = new HttpClient();
            _mainObj = new Lazy<Task<dynamic>>(GetMaiApinObj);
        }
        public async Task<IEnumerable<Stream>> GetStreams(int viewerLimit = 0, int countLimit = 1000)
        {
            var parameter = new LimitParameter(countLimit);
            var res = new List<Stream>();
            string streamsUri = (await _mainObj.Value)._links.streams;
            dynamic streams = JObject.Parse(await _httpClient.GetStringAsync(new RequestStringBuilder(streamsUri).WithParameter(parameter)));
            List<StreamJson> page = streams.streams.ToObject<List<StreamJson>>();
            var finished = false;
            while (page.Count > 0)
            {
                foreach (var stream in page)
                {
                    if (stream.viewers < viewerLimit || (countLimit != 0 && res.Count >= countLimit))
                    {
                        finished = true;
                        break;
                    }
                    res.Add((Stream) stream);
                }
                if (finished)
                    break;
                string next = streams._links.next;
                streams = JObject.Parse(await _httpClient.GetStringAsync(next));
                page = streams.streams.ToObject<List<StreamJson>>();
            }
            return res;
        }

        private async Task<dynamic> GetMaiApinObj()
        {
            return JObject.Parse(await _httpClient.GetStringAsync("https://api.twitch.tv/kraken"));
        }
        public async Task<Stream> GetStream(string channelName)
        {
            string streamsUri = (await _mainObj.Value)._links.streams;
            var streamUri = new StringBuilder(streamsUri);
            streamUri.Append($"/{channelName}");
            var streamJson = await _httpClient.GetStringAsync(streamUri.ToString());
            dynamic parsed = JObject.Parse(streamJson);
            return (Stream) parsed.stream.ToObject<StreamJson>();
        }
    }
}