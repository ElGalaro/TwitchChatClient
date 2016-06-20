using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject;
using Ninject.Parameters;
using TwitchChatClient.Dumping;
using TwitchChatClient.Logic;
using TwitchChatClient.WebApi.Ninject;
using TwtichChatClient.Model;

namespace TwitchChatClient.WebApi.Controllers
{
    public class DumperController : ApiController
    {
        public class GetRequestParameter
        {
            public DateTime Start { get; set; }
            public int Duration { get; set; }
            public long StreamId { get; set; }
        }
        private readonly IChatDumper _dumper;
        private readonly IMessageProvider _provider;

        public DumperController(IChatDumper dumper, IMessageProvider provider)
        {
            _dumper = dumper;
            _provider = provider;
        }

        public async Task<IHttpActionResult> Put([FromBody] string arg)
        {
            await Task.Run(() =>
            {
                switch (arg)
                {
                    case "start":
                        if (!_dumper.Active)
                        {
                            _dumper.Start();
                        }
                        break;
                    case "stop":
                        _dumper.Stop();
                        break;
                }
            });
            return Ok();
        }

        public IEnumerable<ChatMessage> Get([FromUri] GetRequestParameter parameter)
        {
            var utc = parameter.Start.ToUniversalTime();
            return _provider.GetMessages(parameter.StreamId, utc, parameter.Duration);
        }
    }
}
