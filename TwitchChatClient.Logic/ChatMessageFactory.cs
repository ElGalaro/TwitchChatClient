using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TwitchChatClient.Logic.Helpers;
using TwtichChatClient.Model;

namespace TwitchChatClient.Logic
{
    public class ChatMessageFactory
    {
        private static Regex rawChatMessageRegex;
        private static Regex badgesRegex;
        private static Regex emotesRegex;

        static ChatMessageFactory()
        {
            rawChatMessageRegex = new Regex(@"(?:color=(?<color>[^;]*?);)?" +
                                            @"(?:display-name=(?<displayname>[^;]*?);)?" +
                                            @"(?:mod=(?<mod>[0-1]);)" +
                                            @"(?:room-id=(?<roomid>\d*?);)" +
                                            @"(?:subscriber=(?<subscriber>[0-1]);)" +
                                            @"(?:turbo=(?<turbo>[0-1]);)" +
                                            @"(?:user-id=(?<userid>\d*?);)" +
                                            @"(?:user-type=(?<usertype>)[^ ]*?) :" +
                                            @"(?<username>\w+)!\k<username>@\k<username>.tmi.twitch.tv PRIVMSG #(?<channel>\w+) :(?<messagebody>.*)", RegexOptions.Compiled);
            badgesRegex = new Regex(@"badges=(?:(\w+)/1[,;])*", RegexOptions.Compiled);
            emotesRegex = new Regex(@"emotes=(?:(?<emoteid>\d+):(?:(?<start>\d+)-(?<end>\d+)[,;/])+)*", RegexOptions.Compiled);
        }
        public static ChatMessage FromRawString(string rawString, DateTime time)
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

            return result;
        }

        private static IEnumerable<string> GetBadges(string rawString)
        {
            return (from Capture capture in badgesRegex.Match(rawString).Groups[1].Captures select capture.Value).ToList();
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
