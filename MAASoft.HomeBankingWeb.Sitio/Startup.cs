using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MAASoft.HomeBankingWeb.Sitio.Startup))]
namespace MAASoft.HomeBankingWeb.Sitio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
