using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwtichChatClient.Model
{
    public abstract class ApiProviderBase : IApiProvider
    {
        private readonly JsonSerializer _serializer;

        protected ApiProviderBase()
        {
            _serializer = JsonSerializer.Create();
        }

        public T GetApi<T>(params string[] arguments) where T: class
        {
            using(var reader = new StringReader(GetJson<T>(arguments)))
            {
                var typeName = typeof (T).Name;
                return JObject.Parse(GetJson<T>(arguments))[typeName.Substring(0, typeName.Length - 5)].Value<T>();
                //return _serializer.Deserialize<T>(new JsonTextReader(reader));
            }
        }

        public async Task<T> GetApiAsync<T>(params string[] arguments) where T : class
        {
            var typeName = typeof(T).Name;
            var jsonString = await GetJsonAsync<T>(arguments);
            return JObject.Parse(jsonString)[typeName.Substring(0, typeName.Length - 4).ToLower()].ToObject<T>();
            //using (var reader = new StringReader(await GetJsonAsync<T>(arguments)))
            //{

            //    //return _serializer.Deserialize<T>(new JsonTextReader(reader));
            //}
        }

        protected abstract string GetJson<T>(params string[] arguments) where T : class;
        protected abstract Task<string> GetJsonAsync<T>(params string[] arguments) where T : class;
    }
}
