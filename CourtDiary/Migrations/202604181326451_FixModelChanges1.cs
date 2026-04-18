namespace CourtDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class FixModelChanges1 : DbMigration
    {
        public override void Up()
        {
            // Remove old primary key
            DropPrimaryKey("dbo.Meetings");

            // Rename existing identity column instead of creating new one
            RenameColumn("dbo.Meetings", "meeting_id", "Id");

            // Add new columns
            AddColumn("dbo.Meetings", "Subtitle", c => c.String(maxLength: 200));
            AddColumn("dbo.Meetings", "CommentsCount", c => c.Int(nullable: false));
            AddColumn("dbo.Meetings", "ViewsCount", c => c.Int(nullable: false));
            AddColumn("dbo.Meetings", "TagText", c => c.String(maxLength: 50));
            AddColumn("dbo.Meetings", "TagColor", c => c.String(maxLength: 50));
            AddColumn("dbo.Meetings", "Category", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Meetings", "EventDate", c => c.DateTime(nullable: false));

            // Modify existing column
            AlterColumn("dbo.Meetings", "Title", c => c.String(nullable: false, maxLength: 200));

            // Add new primary key
            AddPrimaryKey("dbo.Meetings", "Id");

            // Remove old unused columns
            DropColumn("dbo.Meetings", "user_id");
            DropColumn("dbo.Meetings", "start_datetime");
            DropColumn("dbo.Meetings", "end_datetime");
        }

        public override void Down()
        {
            // Add back removed columns
            AddColumn("dbo.Meetings", "end_datetime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Meetings", "start_datetime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Meetings", "user_id", c => c.Int(nullable: false));

            // Remove primary key
            DropPrimaryKey("dbo.Meetings");

            // Rename Id back to original
            RenameColumn("dbo.Meetings", "Id", "meeting_id");

            // Revert column changes
            AlterColumn("dbo.Meetings", "Title", c => c.String());

            // Remove added columns
            DropColumn("dbo.Meetings", "EventDate");
            DropColumn("dbo.Meetings", "Category");
            DropColumn("dbo.Meetings", "TagColor");
            DropColumn("dbo.Meetings", "TagText");
            DropColumn("dbo.Meetings", "ViewsCount");
            DropColumn("dbo.Meetings", "CommentsCount");
            DropColumn("dbo.Meetings", "Subtitle");

            // Restore original primary key
            AddPrimaryKey("dbo.Meetings", "meeting_id");
        }
    }
}