using System.Data.Entity.Migrations;

namespace TwtichChatClient.Model.Migrations
{
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
                        Stream_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Streams", t => t.Stream_Id)
                .Index(t => t.Stream_Id);
            
            CreateTable(
                "dbo.Streams",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Title = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "Stream_Id", "dbo.Streams");
            DropIndex("dbo.ChatMessages", new[] { "Stream_Id" });
            DropTable("dbo.Streams");
            DropTable("dbo.ChatMessages");
        }
    }
}
