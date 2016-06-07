namespace TwitchChatClient.Dumper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Badges = c.String(),
                        Color = c.String(),
                        DisplayName = c.String(),
                        Emotes = c.String(),
                        IsChannelMod = c.Boolean(nullable: false),
                        RoomId = c.Int(nullable: false),
                        IsSubscriber = c.Boolean(nullable: false),
                        IsTurbo = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                        UserType = c.String(),
                        MessageBody = c.String(),
                        Username = c.String(),
                        Time = c.DateTime(nullable: false),
                        Channel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChatMessages");
        }
    }
}
