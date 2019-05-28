namespace GamePlanService2.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GamePlanService2.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GamePlanService2.Models.ApplicationDbContext context)
        {
            context.Events.AddOrUpdate(
                new Models.Event { Description = "First Event", Lat = 43.055730, Lng = -87.886927, Date = new DateTime(2019, 05, 27) }
                );
        }
    }
}
