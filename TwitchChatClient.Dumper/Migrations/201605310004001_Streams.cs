namespace TwitchChatClient.Dumper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Streams : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Streams",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Title = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ChatMessages", "Stream_Id", c => c.Int());
            CreateIndex("dbo.ChatMessages", "Stream_Id");
            AddForeignKey("dbo.ChatMessages", "Stream_Id", "dbo.Streams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "Stream_Id", "dbo.Streams");
            DropIndex("dbo.ChatMessages", new[] { "Stream_Id" });
            DropColumn("dbo.ChatMessages", "Stream_Id");
            DropTable("dbo.Streams");
        }
    }
}
