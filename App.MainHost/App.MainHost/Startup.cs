using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(App.MainHost.Startup))]
namespace App.MainHost
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
