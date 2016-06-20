using Ninject;
using Ninject.Modules;
using TwitchChatClient.Dumping;
using TwitchChatClient.Logic;
using TwtichChatClient.Model;

namespace TwitchChatClient.WebApi.Ninject
{
    public class MainModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMessagesRepository>()
                .To<MessagesRepository>();
            Bind<IApiRequester>()
                .To<HttpApiRequester>();
            Bind<IChatClient>()
                .To<ChatClient>();
            Bind<IChatMessageFactory>()
                .To<ChatMessageFactory>();
            Bind<IMessageHandler>()
                .To<ChatMessageHandler>();
            Bind<IChatDumper>()
                .To<ChatDumper>();
            Bind<IMessagesReadOnlyContext>()
                .To<MessagesContext>();
            Bind<IMessageProvider>()
                .To<MessageProvider>();
        }
    }
}