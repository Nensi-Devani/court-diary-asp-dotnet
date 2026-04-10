namespace CourtDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUserTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        user_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false),
                        email = c.String(nullable: false),
                        password = c.String(nullable: false),
                        is_varified = c.Boolean(nullable: false),
                        avatar = c.String(),
                        office_email = c.String(),
                        office_address = c.String(),
                        office_phone_no = c.String(),
                        role = c.Int(nullable: false),
                        created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.user_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
