using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwtichChatClient.Model
{
    public class ChatMessage
    {
        public IEnumerable<string> Badges { get; set; }
        public string Color { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<EmoteLocationInfo> Emotes { get; set; }
        public bool IsChannelMod { get; set; }
        public int RoomId { get; set; }
        public bool IsSubscriber { get; set; }
        public bool IsTurbo { get; set; }
        public int UserId { get; set; }
        public string UserType { get; set; }
        public string MessageBody { get; set; }
        public string Username { get; set; }
        public DateTime Time { get; set; }
        public string Channel { get; set; }
        public class EmoteLocationInfo
        {
            public int EmoteId { get; set; }
            public int Start { get; set; }
            public int End { get; set; }
        }
    }
}
