using TwtichChatClient.Model;
using Microsoft.EntityFrameworkCore;

namespace TwitchChatClient.Dumper
{
    public class MessagesDataContext : DbContext
    {
        public virtual DbSet<ChatMessageDto> ChatMessages { get; set; }
        public virtual DbSet<StreamDto> Streams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            //builder.UseSqlServer();
        }
    }
}