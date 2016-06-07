using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwtichChatClient.Model
{

    public class MainJson
    {
        public bool identified { get; set; }
        public _Links _links { get; set; }
        public Token token { get; set; }
        public class _Links
        {
            public string user { get; set; }
            public string channel { get; set; }
            public string search { get; set; }
            public string streams { get; set; }
            public string ingests { get; set; }
            public string teams { get; set; }
        }

        public class Token
        {
            public bool valid { get; set; }
            public object authorization { get; set; }
        }
    }
}
