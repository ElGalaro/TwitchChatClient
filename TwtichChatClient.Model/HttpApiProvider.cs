using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace TwtichChatClient.Model
{
    public class HttpApiProvider : ApiProviderBase
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<Type, string> _mappingTypeToUrl; 

        public HttpApiProvider(IDictionary<Type, string> mappingTypeToUrl = null)
        {
            if(mappingTypeToUrl == null)
            {
                _mappingTypeToUrl = new Dictionary<Type, string>
                {
                    {typeof(EmoticonJson), @"https://api.twitch.tv/kraken/chat/emoticons"},
                    {typeof(StreamsJson),  @"https://api.twitch.tv/kraken/streams" },
                    {typeof(StreamJson),   @"https://api.twitch.tv/kraken/streams/{0}" }
                };
            }
            else
            {
                _mappingTypeToUrl = new Dictionary<Type, string>(mappingTypeToUrl);
            }
            _httpClient = new HttpClient();
        }

        protected override string GetJson<T>(params string[] arguments)
        {
            var url = arguments.Length == 0
                ? _mappingTypeToUrl[typeof (T)]
                : string.Format(_mappingTypeToUrl[typeof (T)], arguments);
            return _httpClient.GetStringAsync(url).Result;
        }

        protected override Task<string> GetJsonAsync<T>(params string[] arguments)
        {
            var url = arguments.Length == 0
                ? _mappingTypeToUrl[typeof(T)]
                : string.Format(_mappingTypeToUrl[typeof(T)], arguments);
            return _httpClient.GetStringAsync(url);
        }
    }
}