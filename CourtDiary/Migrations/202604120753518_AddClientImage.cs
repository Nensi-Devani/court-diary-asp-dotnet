namespace CourtDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "ImagePath");
        }
    }
}
