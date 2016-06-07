using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwtichChatClient.Model.Helpers;

namespace TwtichChatClient.Model
{
    public class ChatMessageFactory
    {
        private static Regex rawChatMessageRegex;
        private static Regex badgesRegex;
        private static Regex emotesRegex;
        private static readonly Dictionary<string, Stream> _streamsCache; 

        private IApiProvider _provider;

        static ChatMessageFactory()
        {
            rawChatMessageRegex = new Regex(@"(?:color=(?<color>[^;]*?);)?(?:display-name=(?<displayname>[^;]*?);)?(?:emotes=(?<emotes>[^;]*?);)?(?:mod=(?<mod>[0-1]);)(?:room-id=(?<roomid>\d*?);)(?:subscriber=(?<subscriber>[0-1]);)(?:turbo=(?<turbo>[0-1]);)(?:user-id=(?<userid>\d*?);)(?:user-type=(?<usertype>)\S*?) :(?<username>\w+)!\k<username>@\k<username>.tmi.twitch.tv PRIVMSG #(?<channel>\w+) :(?<messagebody>.*)", RegexOptions.Compiled);
            badgesRegex = new Regex(@"badges=(?:([\w\d/]+)[,;])*", RegexOptions.Compiled);
            emotesRegex = new Regex(@"emotes=(?:(?<emoteid>\d+):(?:(?<start>\d+)-(?<end>\d+)[,;/])+)*", RegexOptions.Compiled);

            _streamsCache = new Dictionary<string, Stream>();
        }

        public ChatMessageFactory(IApiProvider provider)
        {
            _provider = provider;
        }
        public async Task<ChatMessage> FromRawString(string rawString, DateTime time)
        {
            var result = new ChatMessage();
            var match = rawChatMessageRegex.Match(rawString);
            result.Badges = GetBadges(rawString);
            result.DisplayName = match.Groups["displayname"].Value;
            result.IsChannelMod = match.Value.TryParse<bool>(bool.TryParse);
            result.RoomId = match.Groups["roomid"].Value.TryParse<int>(int.TryParse);
            result.IsSubscriber = match.Groups["subscriber"].Value.TryParse<bool>(bool.TryParse);
            result.IsTurbo = match.Groups["turbo"].Value.TryParse<bool>(bool.TryParse);
            result.UserId = match.Groups["userid"].Value.TryParse<int>(int.TryParse);
            result.UserType = match.Groups["usertype"].Value;
            result.Username = match.Groups["username"].Value;
            result.Channel = match.Groups["channel"].Value;
            result.MessageBody = match.Groups["messagebody"].Value;
            result.Emotes = GetEmotes(rawString);
            result.Time = time;
            result.Color = match.Groups["color"].Value;
            if(result.Channel != string.Empty)
            {
                Stream stream;
                if (_streamsCache.ContainsKey(result.Channel))
                {
                    stream = _streamsCache[result.Channel];
                }
                else
                {
                    var streamRaw = _provider.GetApiAsync<StreamJson>(result.Channel).Result;
                    stream = (Stream) streamRaw;
                    _streamsCache.Add(result.Channel, stream);
                }
                result.Stream = stream;
            }
            return result;
        }

        public static ChatMessage FromDto(ChatMessageDto messageDto)
        {
            return new ChatMessage
            {
                Badges = GetBadges(messageDto.Badges),
                Emotes = GetEmotes(messageDto.Emotes),
                Channel = messageDto.Channel,
                Color = messageDto.Color,
                DisplayName = messageDto.DisplayName,
                Id = messageDto.Id,
                IsChannelMod = messageDto.IsChannelMod,
                IsSubscriber = messageDto.IsSubscriber,
                IsTurbo = messageDto.IsTurbo,
                MessageBody = messageDto.MessageBody,
                RoomId = messageDto.RoomId,
                Stream = (Stream) messageDto.Stream,
                UserType = messageDto.UserType,
                UserId = messageDto.UserId,
                Time = messageDto.Time,
                Username = messageDto.Username
            };
        }
        private static IEnumerable<string> GetBadges(string rawString)
        {
            return from Capture capture in badgesRegex.Match(rawString).Groups[1].Captures
                   select capture.Value;
        }

        private static IEnumerable<ChatMessage.EmoteLocationInfo> GetEmotes(string rawString)
        {
            var result = new List<ChatMessage.EmoteLocationInfo>();
            var match = emotesRegex.Match(rawString);
            for (int i = 0; i < match.Groups[1].Captures.Count; i++)
            {
                result.Add(new ChatMessage.EmoteLocationInfo
                {
                    EmoteId = match.Groups["emoteid"].Captures[i].Value.TryParse<int>(int.TryParse),
                    End = match.Groups["end"].Captures[i].Value.TryParse<int>(int.TryParse),
                    Start = match.Groups["start"].Captures[i].Value.TryParse<int>(int.TryParse)
                });
            }
            return result;
        }
    }
}
