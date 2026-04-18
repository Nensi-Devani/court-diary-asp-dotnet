namespace CourtDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateClientsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        client_id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        name = c.String(),
                        phone = c.String(),
                        avatar = c.String(),
                        address = c.String(),
                        description = c.String(),
                        created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.client_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Clients");
        }
    }
}
