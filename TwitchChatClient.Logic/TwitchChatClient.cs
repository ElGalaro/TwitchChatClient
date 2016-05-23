using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwtichChatClient.Model;

namespace TwitchChatClient.Logic
{
    public class ChatClient
    {
        private TcpClient _tcpClient;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        private string _channelName;
        private const string Login = "justinfan654321";
        private const string Passwd = "blah";
        private readonly CancellationTokenSource _cts;
        public event NewChatMessageEventHandler NewChatMessage = delegate {};
        public string ChannelName
        {
            get { return _channelName; }
        }

        public ChatClient()
        {
            _tcpClient = new TcpClient("irc.twitch.tv", 6667);
            var networkStream = _tcpClient.GetStream();
            _reader = new StreamReader(networkStream);
            _writer = new StreamWriter(networkStream) {AutoFlush = true};
            _cts = new CancellationTokenSource();
        }

        public void Connect()
        {
            _writer.WriteLine($"PASS {Passwd}");
            _writer.WriteLine($"NICK {Login}");
            _writer.WriteLine($"USER {Login} 8 * :{Login}");
            _writer.WriteLine("CAP REQ :twitch.tv/tags");
            Task.Run(() => Routine(_cts.Token), _cts.Token);
        }

        private async void Routine(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var message = await _reader.ReadLineAsync();
                if (message == "PING :tmi.twitch.tv")
                {
                    _writer.WriteLine("PONG :tmi.twitch.tv");
                }
                else
                {
                    var msg = ChatMessageFactory.FromRawString(message, DateTime.Now);
                    if(msg.MessageBody != string.Empty)
                    {
                        NewChatMessage.Invoke(this, new NewChatMessageEventArgs
                        {
                            Message = msg
                        });
                    }
                }
            }
        }

        public void Disconnect()
        {
            _cts.Cancel();
        }

        public void JoinChannel(string channelName)
        {
            _writer.WriteLine($"JOIN #{channelName.ToLower()}");
            _channelName = channelName;
        }
    }

    public delegate void NewChatMessageEventHandler(object sender, NewChatMessageEventArgs args);

    public class NewChatMessageEventArgs
    {
        public ChatMessage Message { get; set; }
    }
}
