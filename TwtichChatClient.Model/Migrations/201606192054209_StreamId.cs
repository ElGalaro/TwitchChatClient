namespace TwtichChatClient.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StreamId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChatMessages", "Stream_Id", "dbo.Streams");
            DropIndex("dbo.ChatMessages", new[] { "Stream_Id" });
            AddColumn("dbo.ChatMessages", "StreamId", c => c.Long(nullable: false));
            DropColumn("dbo.ChatMessages", "Stream_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChatMessages", "Stream_Id", c => c.Long());
            DropColumn("dbo.ChatMessages", "StreamId");
            CreateIndex("dbo.ChatMessages", "Stream_Id");
            AddForeignKey("dbo.ChatMessages", "Stream_Id", "dbo.Streams", "Id");
        }
    }
}
