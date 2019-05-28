namespace GamePlanService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Lat", c => c.Double(nullable: false));
            AddColumn("dbo.Events", "Lng", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "Lng");
            DropColumn("dbo.Events", "Lat");
        }
    }
}
