using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(passionProject1.Startup))]
namespace passionProject1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
