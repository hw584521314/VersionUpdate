using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VersionServer.Startup))]
namespace VersionServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
