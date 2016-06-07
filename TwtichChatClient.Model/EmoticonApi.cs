namespace TwtichChatClient.Model
{
    public class EmoticonJson
    {
        public _Links _links { get; set; }
        public Emoticon[] emoticons { get; set; }
        public class _Links
        {
            public string self { get; set; }
        }

        public class Emoticon
        {
            public string regex { get; set; }
            public Image[] images { get; set; }
        }

        public class Image
        {
            public int? width { get; set; }
            public int? height { get; set; }
            public string url { get; set; }
            public int? emoticon_set { get; set; }
        }
    }

}
