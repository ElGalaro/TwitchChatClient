using System.ComponentModel.DataAnnotations.Schema;
using TwtichChatClient.Model;

namespace TwitchChatClient.Dumper
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MessagesDataContext : DbContext
    {
        public virtual DbSet<ChatMessageDto> ChatMessages { get; set; }
        public virtual DbSet<StreamDto> Streams { get; set; }
        public MessagesDataContext()
            : base("name=MessagesDataContext")
        {
            
        }

    }
}