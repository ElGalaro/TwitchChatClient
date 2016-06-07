using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwtichChatClient.Model;

namespace TwitchChatClient.Logic
{
    public class StreamsInfoProvider
    {
        private readonly IApiProvider _provider;

        public StreamsInfoProvider(IApiProvider provider)
        {
            _provider = provider;
        }

        public IEnumerable<string> GetTopChannels(int count)
        {
            return null;
        } 
    }
}
