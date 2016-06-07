using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using TwitchChatClient.Dumper;
using TwitchChatClient.Logic;
using TwtichChatClient.Model;

namespace TwitchChatClient.Ui.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private ChatClient _client;
        private ChatDumper _dumper;
        public ObservableCollection<ChatMessage> Messages { get; } 
        public ChatViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>();
            _client = new ChatClient(new ChatMessageHandler());
            _client.NewChatMessage += (sender, args) =>
            {
                Application.Current.Dispatcher.InvokeAsync(() => Messages.Add(args.Message));
            };
            _dumper = new ChatDumper(_client);
            Messenger.Default.Register<string>(this, "Disconnect", args => _client.Disconnect());
            Messenger.Default.Register<string>(this, "Join", channel =>
            {
                _client.JoinChannel(channel);
            });
            
            if(!IsInDesignMode)
            {
                _client.Connect();
                _client.JoinChannels(new [] { "DotaMajor", "DotaMajorRU", "nl_Kripp", "summit1g", "TimTheTatman" });
            }
        }
    }
}
