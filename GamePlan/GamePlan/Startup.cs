using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GamePlan.Startup))]
namespace GamePlan
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
