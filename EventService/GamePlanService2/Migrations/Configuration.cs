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
                new Models.Event { Description = "First Event", Category = "Sample" ,Lat = 43.055730, Lng = -87.886927, Temp = 65, Date = new DateTime(2019, 05, 27), EmailNotification = true, Invite = "ryanmkunz@gmail.com" }
                );
            context.Events.AddOrUpdate(
                new Models.Event { Description = "Second Event", Category = "Sample", Lat = 43.055730, Lng = -87.886927, Temp = 65, Date = new DateTime(2019, 05, 27), EmailNotification = true }
                );
            context.Events.AddOrUpdate(
                new Models.Event { Description = "Third Event", Category = "Sample", Lat = 43.055730, Lng = -87.886927, Date = new DateTime(2019, 05, 27), EmailNotification = false, Invite = "ryanmkunz@gmail.com" }
                );
            context.Events.AddOrUpdate(
                new Models.Event { Description = "Eggs", Category = "Groceries", Lat = 43.055730, Lng = -87.886927, Date = new DateTime(2019, 05, 27), EmailNotification = true, Invite = "ryanmkunz@gmail.com" }
                );
            context.Events.AddOrUpdate(
                new Models.Event { Description = "Milk", Category = "Groceries", Lat = 43.055730, Lng = -87.886927, Date = new DateTime(2019, 05, 27), EmailNotification = true }
                );
            context.Events.AddOrUpdate(
                new Models.Event { Description = "Bacon", Category = "Groceries", Lat = 43.055730, Lng = -87.886927, Date = new DateTime(2019, 05, 27), EmailNotification = false, Invite = "ryanmkunz@gmail.com" }
                );
            context.ToDoLists.AddOrUpdate(
                new Models.ToDoList { Title = "To-Do One" , Category = "Sample" }
                );
            context.ToDoLists.AddOrUpdate(
                new Models.ToDoList { Title = "Groceries", Category = "Groceries" }
                );

        }
    }
}
