//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
//using TwitchChatClient.Dumper.Annotations;
//using TwitchChatClient.Logic;
//using TwtichChatClient.Model;

//namespace TwitchChatClient.Dumper
//{
//    public class ChatDumper : INotifyPropertyChanged
//    {
//        private readonly IChatClient _chatClient;
//        private MessagesDataContext _context;
//        private bool _isSaving;
//        private TimeSpan _lastSaveTime;

//        public int CommitCount { get; set; }

//        public bool IsSaving
//        {
//            get { return _isSaving; }
//            set
//            {
//                if (value == _isSaving) return;
//                _isSaving = value;
//                OnPropertyChanged();
//            }
//        }

//        public TimeSpan LastSaveTime
//        {
//            get { return _lastSaveTime; }
//            set
//            {
//                if (value.Equals(_lastSaveTime)) return;
//                _lastSaveTime = value;
//                OnPropertyChanged();
//            }
//        }

//        public ChatDumper(IChatClient chatClient)
//        {
//            _chatClient = chatClient;
//        }

//        public void Start()
//        {
//            _context = new MessagesDataContext();
//            _context.Configuration.AutoDetectChangesEnabled = false;
//            CommitCount = 100;
//            _chatClient.NewChatMessage += ChatClientOnNewChatMessage;
//        }

//        public void Stop()
//        {
//            _context.SaveChanges();
//            _context.Dispose();
//            _chatClient.NewChatMessage -= ChatClientOnNewChatMessage;
//        }

//        private void ChatClientOnNewChatMessage(object sender, NewChatMessageEventArgs args)
//        {
//            var msg = (ChatMessageDto)args.Message;
//            var stream = _context.Streams.Find(msg.Stream.Id);
//            if (stream != null)
//            {
//                msg.Stream = stream;
//            }
//            _context.ChatMessages.Add(msg);
//            if (_context.ChatMessages.Local.Count <= CommitCount)
//                return;
//            IsSaving = true;
//            var sw = new Stopwatch();
//            sw.Start();
//            _context.SaveChanges();
//            sw.Stop();
//            LastSaveTime = sw.Elapsed;
//            _context.Dispose();
//            _context = new MessagesDataContext();
//            _context.Configuration.AutoDetectChangesEnabled = false;
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        [NotifyPropertyChangedInvocator]
//        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}
