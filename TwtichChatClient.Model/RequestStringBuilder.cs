using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwtichChatClient.Model
{
    public class RequestStringBuilder
    {
        private StringBuilder _resultRequest;
        private readonly List<IApiParameter> _parameters;

        public string Result
        {
            get
            {
                _resultRequest = Build();
                return _resultRequest.ToString();
            }
        }

        private StringBuilder Build()
        {
            if (_parameters.Count > 0)
            {
                var paramString = string.Join("&",
                    _parameters.Select(parameter => $"{parameter.Name}={parameter.Value}"));
                _resultRequest.Append("?");
                _resultRequest.Append(paramString);
            }
            return _resultRequest;
        }

        public RequestStringBuilder(string baseUri)
        {
            _resultRequest = new StringBuilder(baseUri);
            _parameters = new List<IApiParameter>();
        }
        public RequestStringBuilder WithParameter(IApiParameter parameter)
        {
            _parameters.Add(parameter);
            return this;
        }

        public static implicit operator string(RequestStringBuilder builder)
        {
            return builder.Result;

        }
    }
}
