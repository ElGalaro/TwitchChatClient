using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwtichChatClient.Model;

namespace TwitchChatClient.Dumping
{
    public interface IChatDumper : IDisposable
    {
        void Start();
        void Stop();
        bool Active { get; }
        Task JoinTopViewerCountChannelsAsync(int viewerLimit);
        Task JoinChannelAsync(string channelName);
        DateTime LastSaveTime { get; }
    }
}