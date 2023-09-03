using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MasterTrade.Startup))]
namespace MasterTrade
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
