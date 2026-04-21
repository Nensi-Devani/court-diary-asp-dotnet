namespace CourtDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixModelChanges2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Cases", "user_id");
            CreateIndex("dbo.Clients", "user_id");
            AddForeignKey("dbo.Clients", "user_id", "dbo.Users", "user_id", cascadeDelete: true);
            AddForeignKey("dbo.Cases", "user_id", "dbo.Users", "user_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cases", "user_id", "dbo.Users");
            DropForeignKey("dbo.Clients", "user_id", "dbo.Users");
            DropIndex("dbo.Clients", new[] { "user_id" });
            DropIndex("dbo.Cases", new[] { "user_id" });
        }
    }
}
