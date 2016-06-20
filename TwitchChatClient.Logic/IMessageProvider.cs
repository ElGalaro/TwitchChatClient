using System;
using System.Collections.Generic;
using TwtichChatClient.Model;

namespace TwitchChatClient.Logic
{
    public interface IMessageProvider
    {
        IEnumerable<ChatMessage> GetMessages(long streamId, DateTime start, int duartionsSeconds);
    }
}