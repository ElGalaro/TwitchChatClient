using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchChatClient.Logic;
using TwtichChatClient.Model;

namespace TwitchChatClient.Dumper
{
    public class ChatDumper
    {
        private readonly IChatClient _chatClient;
        private MessagesDataContext _context;
        private int _counter;

        public int CommitCount { get; set; }
        public ChatDumper(IChatClient chatClient)
        {
            _chatClient = chatClient;
            _context = new MessagesDataContext();
            _context.Configuration.AutoDetectChangesEnabled = false;
            CommitCount = 100;
            _chatClient.NewChatMessage += ChatClientOnNewChatMessage;
        }

        private void ChatClientOnNewChatMessage(object sender, NewChatMessageEventArgs args)
        {
            var msg = (ChatMessageDto) args.Message;
            var stream = _context.Streams.Find(msg.Stream.Id);
            if (stream != null)
            {
                msg.Stream = stream;
            }
            _context.ChatMessages.Add(msg);
            if (_counter > CommitCount)
            {
                _context.SaveChanges();
                _context.Dispose();
                _context = new MessagesDataContext();
                _context.Configuration.AutoDetectChangesEnabled = false;
                _counter = 0;
            }
            else
            {
                _counter++;
            }
        }
    }
}
