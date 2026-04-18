
namespace CourtDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMissingTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Case_Party",
                c => new
                    {
                        party_id = c.Int(nullable: false, identity: true),
                        case_id = c.Int(nullable: false),
                        name = c.String(),
                        type = c.String(),
                        notes = c.String(),
                        avatar = c.String(),
                    })
                .PrimaryKey(t => t.party_id);
            
            CreateTable(
                "dbo.Hearings",
                c => new
                    {
                        hearing_id = c.Int(nullable: false, identity: true),
                        case_id = c.Int(nullable: false),
                        hearing_date = c.DateTime(),
                        next_hearing_date = c.DateTime(),
                        notes = c.String(),
                        status = c.String(),
                        fee_amount = c.Decimal(precision: 18, scale: 2),
                        payment_status = c.String(),
                        payment_date = c.DateTime(),
                        payment_method = c.String(),
                    })
                .PrimaryKey(t => t.hearing_id);
            
            CreateTable(
                "dbo.Meetings",
                c => new
                    {
                        meeting_id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        title = c.String(),
                        start_datetime = c.DateTime(nullable: false),
                        end_datetime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.meeting_id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        notification_id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        title = c.String(),
                        message = c.String(),
                        is_read = c.Boolean(nullable: false),
                        created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.notification_id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notifications");
            DropTable("dbo.Meetings");
            DropTable("dbo.Hearings");
            DropTable("dbo.Case_Party");
        }
    }
}
