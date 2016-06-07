using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwtichChatClient.Model;

namespace TwitchChatClient.Logic
{
    public class ChatMessageHandler : IMessageHandler
    {
        private readonly ChatMessageFactory _factory;
        public IChatClient ChatClient { get; set; }
        public event NewChatMessageEventHandler NewChatMessage = delegate { };

        public ChatMessageHandler()
        {
            _factory = new ChatMessageFactory(new HttpApiProvider());
        }

        public async void Handle(string message)
        {
            if (message == "PING :tmi.twitch.tv")
            {
                if (ChatClient != null)
                {
                    await ChatClient.WriteRaw("PONG :tmi.twitch.tv");
                }
                return;
            }
            var msg = await _factory.FromRawString(message, DateTime.Now);
            if (msg.MessageBody != string.Empty)
            {
                NewChatMessage.Invoke(this, new NewChatMessageEventArgs
                {
                    Message = msg
                });
                Debug.WriteLine(message);
            }
        }
    }



    public interface IMessageHandler
    {
        void Handle(string rawMessage);
        event NewChatMessageEventHandler NewChatMessage;
        IChatClient ChatClient { get; set; }
    }
}
