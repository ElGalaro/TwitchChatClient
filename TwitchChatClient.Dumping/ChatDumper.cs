using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TwitchChatClient.Dumping.Annotations;
using TwitchChatClient.Logic;
using TwtichChatClient.Model;
using Timer = System.Timers.Timer;

namespace TwitchChatClient.Dumping
{
    public class ChatDumper : IChatDumper, INotifyPropertyChanged
    {
        private readonly IChatClient _client;
        private readonly IMessagesRepository _repository;
        private readonly IApiRequester _apiRequester;
        private Timer _routineTimer;
        public bool Active { get; private set; }
        public bool Connected => _client.Connected;
        public DateTime LastSaveTime { get; private set; }
        
        public int RefreshIntervalSeconds { get; set; }

        public ChatDumper(IChatClient client, IMessagesRepository repository, IApiRequester apiRequester)
        {
            _client = client;
            _repository = repository;
            _apiRequester = apiRequester;
            RefreshIntervalSeconds = 60;
            _routineTimer = new Timer(RefreshIntervalSeconds * 1000);
            _routineTimer.Elapsed += RefreshRoutineInternal;
        }

        private async void RefreshRoutineInternal(object sender, ElapsedEventArgs args)
        {
            var sw = new Stopwatch();
            sw.Start();
            _client.Synchronize(await _apiRequester.GetStreams(1000, 0));
            sw.Stop();
            Debug.WriteLine($"Synchronized. Took {sw.ElapsedMilliseconds}ms");
        }
        public void Start()
        {
            if(!_client.Connected)
                _client.Connect();
            _routineTimer.Start();
            RefreshRoutineInternal(this, null);
            _client.NewChatMessage += ClientOnNewChatMessage;
            _repository.SaveCompleted += async (sender, time) => await Task.Run(() =>
            {
                LastSaveTime = time;
                OnPropertyChanged(nameof(LastSaveTime));
            });
            Active = true;
        }

        private void ClientOnNewChatMessage(object sender, NewChatMessageEventArgs args)
        {
            _repository.Add(args.Message);
        }

        public void Stop()
        {
            _client.NewChatMessage -= ClientOnNewChatMessage;
            _routineTimer.Stop();
            Active = false;
        }

        public async Task JoinTopViewerCountChannelsAsync(int viewerLimit)
        {
            if(!_client.Connected)
                _client.Connect();
            _client.JoinChannels(await _apiRequester.GetStreams(viewerLimit, 0));
        }

        public async Task JoinChannelAsync(string channelName)
        {
            if (!_client.Connected)
                _client.Connect();
            _client.JoinChannel(await _apiRequester.GetStream(channelName));
        }

        public void Dispose()
        {
            Stop();
            _client.Dispose();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
