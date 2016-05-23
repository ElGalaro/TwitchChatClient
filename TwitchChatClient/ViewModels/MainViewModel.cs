using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace TwitchChatClient.Ui.ViewModels
{
    public class MainViewModel
    {
        public RelayCommand StopCommand { get; private set; }
        public RelayCommand<string> JoinCommand { get; private set; }

        public MainViewModel()
        {
            StopCommand = new RelayCommand(() => Messenger.Default.Send("", "Disconnect"));
            JoinCommand = new RelayCommand<string>(channel => Messenger.Default.Send(channel, "Join"));
        }
    }
}
