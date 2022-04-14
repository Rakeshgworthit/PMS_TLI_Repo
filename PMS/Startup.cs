using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PMS.Startup))]
namespace PMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
