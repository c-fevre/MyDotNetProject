using log4net.Config;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyContractsGenerator.WebUI.Startup))]

namespace MyContractsGenerator.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
            XmlConfigurator.Configure();
        }
    }
}