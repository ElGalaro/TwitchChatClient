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
using TwitchChatClient.Logic;
using TwtichChatClient.Model;

namespace TwitchChatClient.Ui.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private ChatClient _client;
        public ObservableCollection<ChatMessage> Messages { get; } 
        public ChatViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>();
            _client = new ChatClient();
            _client.NewChatMessage += (sender, args) =>
            {
                Application.Current.Dispatcher.InvokeAsync(() => Messages.Add(args.Message));
            };
            Messenger.Default.Register<string>(this, "Disconnect", args =>_client.Disconnect());
            Messenger.Default.Register<string>(this, "Join", channel => _client.JoinChannel(channel));
            if(!IsInDesignMode)
                _client.Connect();
        }
    }
}
