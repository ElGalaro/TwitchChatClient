using System;
using System.Collections.Generic;
using System.Linq;
using TwtichChatClient.Model;

namespace TwitchChatClient.Logic
{
    public class MessageProvider : IMessageProvider
    {
        private readonly IMessagesReadOnlyContext _context;

        public MessageProvider(IMessagesReadOnlyContext context)
        {
            _context = context;
        }

        public IEnumerable<ChatMessage> GetMessages(long streamId, DateTime start, int duartionsSeconds)
        {
            var end = start.AddSeconds(duartionsSeconds);
            return _context.MessagesQueryable
                .Where(msg => msg.StreamId == streamId && msg.Time >= start && msg.Time <= end)
                .ToList()
                .Select(m => (ChatMessage) m);
        }
    }
}