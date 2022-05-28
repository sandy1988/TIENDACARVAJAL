using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SandyStore.Startup))]
namespace SandyStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
