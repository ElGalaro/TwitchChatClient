using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwtichChatClient.Model
{

    public class StreamsJson
    {
        public StreamJson[] streams { get; set; }
        public int _total { get; set; }
        public _Links _links { get; set; }

        public class _Links
        {
            public string self { get; set; }
            public string next { get; set; }
            public string featured { get; set; }
            public string summary { get; set; }
            public string followed { get; set; }
        }
    }

    public class StreamRoot
    {
        public StreamJson stream { get; set; }
        public _Links2 _links { get; set; }

        public class _Links2
        {
            public string self { get; set; }
            public string channel { get; set; }
        }

    }
    [JsonObject("stream")]
    public class StreamJson
    {
        public long _id { get; set; }
        public string game { get; set; }
        public int viewers { get; set; }
        public DateTime created_at { get; set; }
        public int video_height { get; set; }
        public float average_fps { get; set; }
        public int delay { get; set; }
        public bool is_playlist { get; set; }
        public _Links _links { get; set; }
        public Preview preview { get; set; }
        public Channel channel { get; set; }

        public class _Links
        {
            public string self { get; set; }
        }

        public class Preview
        {
            public string small { get; set; }
            public string medium { get; set; }
            public string large { get; set; }
            public string template { get; set; }
        }

        public class Channel
        {
            public bool? mature { get; set; }
            public string status { get; set; }
            public string broadcaster_language { get; set; }
            public string display_name { get; set; }
            public string game { get; set; }
            public string language { get; set; }
            public int _id { get; set; }
            public string name { get; set; }
            public DateTime created_at { get; set; }
            public DateTime updated_at { get; set; }
            public object delay { get; set; }
            public string logo { get; set; }
            public object banner { get; set; }
            public string video_banner { get; set; }
            public object background { get; set; }
            public object profile_banner { get; set; }
            public object profile_banner_background_color { get; set; }
            public bool partner { get; set; }
            public string url { get; set; }
            public int views { get; set; }
            public int followers { get; set; }
            public _Links1 _links { get; set; }
            public class _Links1
            {
                public string self { get; set; }
                public string follows { get; set; }
                public string commercial { get; set; }
                public string stream_key { get; set; }
                public string chat { get; set; }
                public string features { get; set; }
                public string subscriptions { get; set; }
                public string editors { get; set; }
                public string teams { get; set; }
                public string videos { get; set; }
            }
        }

        public static explicit operator Stream(StreamJson json)
        {
            if (json == null)
                return null;
            return new Stream
            {
                CreatedAt = json.created_at,
                Id = json._id,
                Title = json.channel.status,
                ChannelName = json.channel.name
            };
        }
    }

}
