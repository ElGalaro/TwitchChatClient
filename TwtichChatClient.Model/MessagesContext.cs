using System.Data.Entity;
using System.Linq;

namespace TwtichChatClient.Model
{
    public class MessagesContext : DbContext, IMessagesReadOnlyContext
    {
        public MessagesContext() : base("MS_TableConnectionString")
        {
            
        }
        public DbSet<ChatMessageDto> Messages { get; set; }
        public DbSet<StreamDto> Streams { get; set; }
        public IQueryable<ChatMessageDto> MessagesQueryable => Messages;
        public IQueryable<StreamDto> StreamsQueryable => Streams;
    }

    public interface IMessagesReadOnlyContext
    {
        IQueryable<ChatMessageDto> MessagesQueryable { get; }
        IQueryable<StreamDto> StreamsQueryable { get; } 
    }
}
