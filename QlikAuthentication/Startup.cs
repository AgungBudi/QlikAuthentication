using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QlikAuthentication.Startup))]
namespace QlikAuthentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
