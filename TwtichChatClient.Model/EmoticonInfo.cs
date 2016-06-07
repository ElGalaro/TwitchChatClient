using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwtichChatClient.Model
{
    [Serializable]
    public class EmoticonInfo
    {
        public string Url { get; set; }
        public byte[] Image { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public string Regex { get; set; }
    }
}
