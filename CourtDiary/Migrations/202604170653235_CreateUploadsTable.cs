namespace CourtDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateUploadsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Uploads",
                c => new
                    {
                        upload_id = c.Int(nullable: false, identity: true),
                        case_id = c.Int(nullable: false),
                        upload_url = c.String(nullable: false),
                        upload_category = c.Int(nullable: false),
                        created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.upload_id)
                .ForeignKey("dbo.Cases", t => t.case_id, cascadeDelete: true)
                .Index(t => t.case_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Uploads", "case_id", "dbo.Cases");
            DropIndex("dbo.Uploads", new[] { "case_id" });
            DropTable("dbo.Uploads");
        }
    }
}
