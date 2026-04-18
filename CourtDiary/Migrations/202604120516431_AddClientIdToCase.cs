namespace CourtDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientIdToCase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cases", "client_id", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cases", "client_id");
        }
    }
}
