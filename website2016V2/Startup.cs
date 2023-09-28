using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(website2016V2.Startup))]
namespace website2016V2
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
