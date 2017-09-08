using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DBTProject.Startup))]
namespace DBTProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
