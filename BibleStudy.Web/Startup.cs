using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BibleStudy.Web.Startup))]
namespace BibleStudy.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
