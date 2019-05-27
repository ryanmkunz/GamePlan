using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GamePlanService.Startup))]
namespace GamePlanService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
