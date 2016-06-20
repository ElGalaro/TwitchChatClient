using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwtichChatClient.Model
{
    public class LimitParameter : IApiParameter
    {
        private const int _max = 100;
        public string Name => "limit";
        public string Value { get; }
        public int Max => _max;

        public LimitParameter(int value)
        {
            var res = (value > Max || value == 0) ? Max : value;
            Value = res.ToString();
        }
    }
}
