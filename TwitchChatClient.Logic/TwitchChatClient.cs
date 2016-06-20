using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwtichChatClient.Model;
using Stream = TwtichChatClient.Model.Stream;

namespace TwitchChatClient.Logic
{
    public class ChatClient : IChatClient
    {
        //TODO List<IMessageHandler>
        private readonly IMessageHandler _handler;
        private TcpClient _tcpClient;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        private const string Login = "justinfan654321";
        private const string Passwd = "blah";
        private readonly CancellationTokenSource _cts;
        private ConcurrentDictionary<Stream, byte> _joinedStreams;
        public event NewChatMessageEventHandler NewChatMessage = delegate {};

        public IEnumerable<Stream> JoinedStreams => _joinedStreams.Keys;
        public bool Connected { get; private set; }
        public ChatClient(IMessageHandler handler)
        {
            _tcpClient = new TcpClient("irc.twitch.tv", 6667);
            var networkStream = _tcpClient.GetStream();
            _reader = new StreamReader(networkStream);
            _writer = new StreamWriter(networkStream) {AutoFlush = true};
            _cts = new CancellationTokenSource();
            _joinedStreams = new ConcurrentDictionary<Stream, byte>();
            _handler = handler;
            _handler.ChatClient = this;
            _handler.NewChatMessage += (sender, args) => NewChatMessage.Invoke(sender, args);
        }

        public void Connect()
        {
            _writer.WriteLine($"PASS {Passwd}");
            _writer.WriteLine($"NICK {Login}");
            _writer.WriteLine($"USER {Login} 8 * :{Login}");
            _writer.WriteLine("CAP REQ :twitch.tv/tags");
            Task.Run(() => Routine(_cts.Token), _cts.Token);
            Connected = true;
        }

        private async void Routine(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var message = await _reader.ReadLineAsync();
                _handler.Handle(message);
            }
        }

        public void Disconnect()
        {
            _cts.Cancel();
            Connected = false;
        }

        public void JoinChannel(Stream channel)
        {
            _writer.WriteLine($"JOIN #{channel.ChannelName.ToLower()}");
            _joinedStreams.TryAdd(channel, new byte());
        }
        public void LeaveChannel(Stream channel)
        {
            _writer.WriteLine($"PART {channel.ChannelName}");
            byte temp;
            _joinedStreams.TryRemove(channel, out temp);
        }
        public void JoinChannels(IEnumerable<Stream> channels)
        {
            foreach (var channel in channels)
            {
                JoinChannel(channel);
            }
        }

        public async Task WriteRaw(string rawMessage)
        {
            await _writer.WriteLineAsync(rawMessage);
        }

        public void Dispose()
        {
            Disconnect();
        }
        /// <summary>
        /// Synchronizes joined streams with the provided argument.
        /// </summary>
        /// <param name="streams"></param>
        public void Synchronize(IEnumerable<Stream> streams)
        {
            var hashSetTarget = new HashSet<Stream>(streams);
            var leave = new List<Stream>();
            var join = new List<Stream>();
            foreach (var stream in hashSetTarget.Union(_joinedStreams.Keys))
            {
                if (!_joinedStreams.ContainsKey(stream) && hashSetTarget.Contains(stream))
                {
                    join.Add(stream);
                }
                if (!hashSetTarget.Contains(stream) && _joinedStreams.ContainsKey(stream))
                {
                    leave.Add(stream);
                }
            }
            leave.ForEach(LeaveChannel);
            join.ForEach(JoinChannel);
        }
    }
    public interface IChatClient : IDisposable
    {
        Task WriteRaw(string rawMessage);
        event NewChatMessageEventHandler NewChatMessage;
        void Connect();
        void JoinChannel(Stream channel);
        void JoinChannels(IEnumerable<Stream> channels);
        void LeaveChannel(Stream channel);
        void Synchronize(IEnumerable<Stream> streams);
        bool Connected { get; }
    }

    public delegate void NewChatMessageEventHandler(object sender, NewChatMessageEventArgs args);

    public class NewChatMessageEventArgs
    {
        public ChatMessage Message { get; set; }
        public string Raw { get; set; }
    }
}
