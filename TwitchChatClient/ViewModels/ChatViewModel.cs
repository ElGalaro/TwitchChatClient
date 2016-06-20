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
using TwitchChatClient.Dumping;
using TwitchChatClient.Logic;
using TwtichChatClient.Model;

namespace TwitchChatClient.Ui.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private readonly ChatClient _client;
        private readonly ChatDumper _dumper;
        public ObservableCollection<ChatMessage> Messages { get; }
        public DateTime LastSaveTime => _dumper.LastSaveTime;
        public ChatViewModel()
        {
            
        }
    }
}
