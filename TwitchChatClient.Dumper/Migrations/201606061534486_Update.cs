namespace TwitchChatClient.Dumper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChatMessages", "Stream_Id", "dbo.Streams");
            DropIndex("dbo.ChatMessages", new[] { "Stream_Id" });
            DropPrimaryKey("dbo.Streams");
            AlterColumn("dbo.ChatMessages", "Stream_Id", c => c.Long());
            AlterColumn("dbo.Streams", "Id", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.Streams", "Id");
            CreateIndex("dbo.ChatMessages", "Stream_Id");
            AddForeignKey("dbo.ChatMessages", "Stream_Id", "dbo.Streams", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatMessages", "Stream_Id", "dbo.Streams");
            DropIndex("dbo.ChatMessages", new[] { "Stream_Id" });
            DropPrimaryKey("dbo.Streams");
            AlterColumn("dbo.Streams", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.ChatMessages", "Stream_Id", c => c.Int());
            AddPrimaryKey("dbo.Streams", "Id");
            CreateIndex("dbo.ChatMessages", "Stream_Id");
            AddForeignKey("dbo.ChatMessages", "Stream_Id", "dbo.Streams", "Id");
        }
    }
}
