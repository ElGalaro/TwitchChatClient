using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityFramework.BulkInsert.Extensions;
using TwtichChatClient.Model;

namespace TwitchChatClient.Dumping
{
    public class MessagesRepository : IMessagesRepository
    {
        private List<ChatMessageDto> _pendingMessages; 
        private MessagesContext _streamCheck;
        public AutoSaveOptions AutoSaveOptions { get; }
        public event EventHandler<DateTime> SaveCompleted = delegate {};
        private volatile bool _saving;
        private readonly object _syncRoot;
        public MessagesRepository()
        {
            _pendingMessages = new List<ChatMessageDto>();
            AutoSaveOptions = new AutoSaveOptions();
            _streamCheck = new MessagesContext();
            _syncRoot = new object();
        }

        public async void Add(ChatMessage message)
        {
            lock(_syncRoot)
            {
                _pendingMessages.Add((ChatMessageDto)message);
            }
            if (AutoSaveOptions.Enabled)
            {
                if (_pendingMessages.Count >= AutoSaveOptions.CommitCount && !_saving)
                {
                    await SaveChangesAsync();
                }
            }
        }

        public void SaveChanges()
        {
            SaveChangesAsync().Wait();
        }

        public async Task SaveChangesAsync()
        {
            _saving = true;
            List<ChatMessageDto> temp;
            lock (_syncRoot)
            {
                temp = _pendingMessages;
                _pendingMessages = new List<ChatMessageDto>();
            }
            var context = new MessagesContext();
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;
            // ReSharper disable once AccessToDisposedClosure
            await Task.Run(() => { context.BulkInsert(temp); });
            context.Dispose();
            SaveCompleted.Invoke(this, DateTime.Now);
            _saving = false;
        }
    }

    public class AutoSaveOptions
    {
        public bool Enabled { get; set; }
        public int CommitCount { get; set; }

        public AutoSaveOptions()
        {
            Enabled = true;
            CommitCount = 500;
        }
    }

    public interface IMessagesRepository
    {
        void Add(ChatMessage message);
        void SaveChanges();
        Task SaveChangesAsync();
        event EventHandler<DateTime> SaveCompleted;
    }
}
