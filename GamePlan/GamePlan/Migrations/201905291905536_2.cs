namespace GamePlan.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Invite", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Invite");
        }
    }
}
