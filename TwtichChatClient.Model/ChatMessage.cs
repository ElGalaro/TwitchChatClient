using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwtichChatClient.Model
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public Stream Stream { get; set; }
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

        public static explicit operator ChatMessageDto(ChatMessage message)
        {
            return new ChatMessageDto
            {
                Id = message.Id,
                //Stream = (StreamDto) message.Stream,
                StreamId = message.Stream.Id,
                Color = message.Color,
                DisplayName = message.DisplayName,
                Channel = message.Channel,
                IsChannelMod = message.IsChannelMod,
                IsSubscriber = message.IsSubscriber,
                IsTurbo = message.IsTurbo,
                MessageBody = message.MessageBody,
                Username = message.Username,
                UserType = message.UserType,
                Time = message.Time,
                RoomId = message.RoomId,
                UserId = message.UserId,
                Badges = string.Join(",", message.Badges),
                Emotes = string.Join("/", message.Emotes.Select(emote => $"{emote.EmoteId}:{emote.Start}-{emote.End}"))
            };
        }
    }
    [Table("ChatMessages")]
    public class ChatMessageDto
    {
        public int Id { get; set; }
        //public StreamDto Stream { get; set; }
        public long StreamId { get; set; }
        public string Badges { get; set; }
        public string Color { get; set; }
        public string DisplayName { get; set; }
        public string Emotes { get; set; }
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

        public static explicit operator ChatMessage(ChatMessageDto messageDto)
        {
            return ChatMessageFactory.FromDto(messageDto);
        }
    }
}
