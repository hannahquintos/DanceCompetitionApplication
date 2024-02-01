using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DanceCompetitionApplication.Startup))]
namespace DanceCompetitionApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
