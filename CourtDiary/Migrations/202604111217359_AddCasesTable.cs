namespace CourtDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCasesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cases",
                c => new
                    {
                        case_id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        case_no = c.String(),
                        title = c.String(),
                        description = c.String(),
                        previous_hearing_summary = c.String(),
                        hearing_notes = c.String(),
                        location = c.String(),
                        status = c.String(),
                        created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.case_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Cases");
        }
    }
}
