using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwtichChatClient.Model
{
    public interface IApiRequester
    {
        Task<IEnumerable<Stream>> GetStreams(int viewerLimit, int countLimit);
        Task<Stream> GetStream(string channelName);
    }
}