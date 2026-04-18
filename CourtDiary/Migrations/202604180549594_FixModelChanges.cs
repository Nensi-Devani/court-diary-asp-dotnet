namespace CourtDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixModelChanges : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Case_Party", "case_id");
            CreateIndex("dbo.Cases", "client_id");
            AddForeignKey("dbo.Case_Party", "case_id", "dbo.Cases", "case_id", cascadeDelete: true);
            AddForeignKey("dbo.Cases", "client_id", "dbo.Clients", "client_id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cases", "client_id", "dbo.Clients");
            DropForeignKey("dbo.Case_Party", "case_id", "dbo.Cases");
            DropIndex("dbo.Cases", new[] { "client_id" });
            DropIndex("dbo.Case_Party", new[] { "case_id" });
        }
    }
}
